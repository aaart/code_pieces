using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace PipeSharp
{
    public static class PipelineUtilities
    {

        public static bool Execute<T, TFilteringError>(this Func<IState<T, TFilteringError>> step, out IState<T, TFilteringError> state)
        {
            state = step();
            return !state.Invalid && !state.Broken;
        }

        public static IState<TFilteringError> Decorate<T, TFilteringError>(this Func<IState<T, TFilteringError>> step, Action<IState<T, TFilteringError>> target, Action onDoing, Action onDone) =>
            step.Execute(out IState<T, TFilteringError> state) ? TryCatch(state, target, onDoing, onDone) : state.Fail();

        public static IState<TR, TFilteringError> Decorate<T, TR, TFilteringError>(this Func<IState<T, TFilteringError>> method, Func<IState<T, TFilteringError>, TR> target, Action onDoing, Action onDone) =>
            method.Execute(out IState<T, TFilteringError> state) ? TryCatch(state, target, onDoing, onDone) : state.Fail<TR>();

        public static IState<T, TFilteringError> Decorate<T, TK, TFilteringError>(this Func<IState<T, TFilteringError>> step, Func<T, TK> transform, IFilter<TK, TFilteringError> filter)
        {
            var state = step();
            try
            {
                state.LogDebug($"Validating {typeof(T)} object.");
                TK target = transform(state.Result);
                if (!state.Broken && !filter.Check(target, out TFilteringError error))
                {
                    state.LogError($"{typeof(T)} is invalid. {typeof(TFilteringError)} error registered.");
                    return state.Invalidate(error);
                }
                state.LogDebug("Validated");
                return state.Next(state.Result);
            }
            catch (Exception ex)
            {
                state.LogError($"An exception was thrown during validation of {typeof(T)}.");
                state.LogError(ex, ex.Message);
                return state.Fail<T>(ex);
            }
        }

        public static IState<TFilteringError> TryCatch<T, TFilteringError>(IState<T, TFilteringError> state, Action<IState<T, TFilteringError>> step, Action onDoing, Action onDone)
        {
            try
            {
                state.LogDebug($"Executing step for {typeof(T)}");
                onDoing();
                step(state);
                onDone();
                return state.Next();
            }
            catch (Exception ex)
            {
                state.LogError($"An exception was thrown during execution of step for {typeof(T)}.");
                return state.Fail(ex);
            }
        }

        public static IState<TR, TFilteringError> TryCatch<T, TR, TFilteringError>(IState<T, TFilteringError> state, Func<IState<T, TFilteringError>, TR> step, Action onDoing, Action onDone)
        {
            try
            {
                state.LogDebug($"Executing step for {typeof(T)}");
                onDoing();
                var r = step(state);
                onDone();
                return state.Next(r);
            }
            catch (Exception ex)
            {
                state.LogError($"An exception was thrown during execution of step for {typeof(T)}.");
                return state.Fail<TR>(ex);
            }
        }

        public static TPipelineResult Sink<TPipelineResult, TState, TFilteringError>(this TState state, Action<TPipelineResult, TState> setup = null)
            where TPipelineResult : IPipelineResult<TFilteringError>, new()
            where TState : IState<TFilteringError>
        {
            state.LogDebug("All steps executed. Building result object");
            var result = new TPipelineResult();
            setup?.Invoke(result, state);
            state.Done();
            return result;
        }
    }
}
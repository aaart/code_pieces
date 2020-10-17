using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace PipeSharp.Internal
{
    internal static class PipelineUtilities
    {
        private static bool Execute<T, TError>(this Func<IState<T, TError>> step, out IState<T, TError> state)
        {
            state = step();
            return !state.Invalid && !state.Broken;
        }

        public static IState<TError> Decorate<T, TError>(this Func<IState<T, TError>> step, Action<IState<T, TError>> target, Action onDoing, Action onDone, Action<Exception, ILogger> exceptionHandler) =>
            step.Execute(out IState<T, TError> state) ? TryCatch(state, target, onDoing, onDone, exceptionHandler) : state.Fail();

        public static IState<TR, TError> Decorate<T, TR, TError>(this Func<IState<T, TError>> method, Func<IState<T, TError>, TR> target, Action onDoing, Action onDone, Action<Exception, ILogger> exceptionHandler) =>
            method.Execute(out IState<T, TError> state) ? TryCatch(state, target, onDoing, onDone, exceptionHandler) : state.Fail<TR>();

        public static IState<T, TError> Decorate<T, TK, TError>(this Func<IState<T, TError>> step, Func<T, TK> transform, IFilter<TK, TError> filter, Action<Exception, ILogger> exceptionHandler)
        {
            var state = step();
            try
            {
                state.LogDebug($"Validating {typeof(T)} object.");
                TK target = transform(state.StepResult);
                if (!state.Broken && !filter.Check(target, out TError error))
                {
                    state.LogError($"{typeof(T)} is invalid. {typeof(TError)} error registered.");
                    return state.Invalidate(error);
                }
                state.LogDebug("Validated");
                return state.Next(state.StepResult);
            }
            catch (Exception ex)
            {
                exceptionHandler(ex, state);
                return state.Fail<T>(ex);
            }
        }

        private static IState<TError> TryCatch<T, TError>(IState<T, TError> state, Action<IState<T, TError>> step, Action onDoing, Action onDone, Action<Exception, ILogger> exceptionHandler)
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
                exceptionHandler(ex, state);
                return state.Fail(ex);
            }
        }

        private static IState<TR, TError> TryCatch<T, TR, TError>(IState<T, TError> state, Func<IState<T, TError>, TR> step, Action onDoing, Action onDone, Action<Exception, ILogger> exceptionHandler)
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
                exceptionHandler(ex, state);
                return state.Fail<TR>(ex);
            }
        }

        public static TPipelineSummary Sink<TPipelineSummary, TState, TError>(this TState state, Action<TPipelineSummary, TState> setup = null)
            where TPipelineSummary : IPipelineSummary<TError>, new()
            where TState : IState<TError>
        {
            state.LogDebug("All steps executed. Building result object");
            // TODO: remove parameterless constructor and move setup method to Sink method (here).
            var result = new TPipelineSummary();
            setup?.Invoke(result, state);
            state.Done();
            return result;
        }
        
        public static TError[] MergeExceptionAndErrors<TError>(Exception exception, IEnumerable<Func<Exception, TError>> exceptionToErrorMappers, IEnumerable<TError> errors)
        {
            return exception != null
                ? exceptionToErrorMappers.Select(m => m(exception)).Concat(errors).ToArray()
                : errors.ToArray();
        }
    }
}
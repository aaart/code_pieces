using System;
using System.Collections.Generic;
using System.Linq;

namespace Flow
{
    public static class PipelineUtilities
    {

        public static bool Execute<T>(this Func<IState<T>> step, out IState<T> state)
        {
            state = step();
            return !state.Errors.Any() && !state.Failed;
        }

        public static IState Decorate<T>(this Func<IState<T>> step, Action<IState<T>> target) =>
            step.Execute(out IState<T> state) ? TryCatch(state, target) : state.Fail();

        public static IState<TR> Decorate<T, TR>(this Func<IState<T>> method, Func<IState<T>, TR> target) =>
            method.Execute(out IState<T> state) ? TryCatch(state, target) : state.Fail<TR>();

        public static IState<T> Decorate<T, TK>(this Func<IState<T>> filtering, Func<T, TK> transform, IFilter<TK> filter)
        {
            var state = filtering();
            try
            {
                TK target = transform(state.Result);
                if (!state.Failed && !filter.Check(target, out IFilteringError error))
                {
                    state.PublishError(error);
                }
            }
            catch (Exception ex)
            {
                state.Exception = ex;
            }

            return state;
        }

        public static IState TryCatch<T>(IState<T> state, Action<IState<T>> step)
        {
            try
            {
                step(state);
                return state.Next();
            }
            catch (Exception ex)
            {
                state.Exception = ex;
                return state.Next();
            }
        }

        public static IState<TR> TryCatch<T, TR>(IState<T> state, Func<IState<T>, TR> step)
        {
            try
            {
                var r = step(state);
                return state.Next(r);
            }
            catch (Exception ex)
            {
                state.Exception = ex;
                return state.Next(default(TR));
            }
        }

        public static (T, Exception, IFilteringError[]) Sink<T, TState>(this TState state, Action<T, TState> setup = null)
            where T : IPipelineResult, new()
            where TState : IState
        {
            var result = new T();
            setup?.Invoke(result, state);
            state.Done();
            return (result, state.Exception, state.Errors.ToArray());
        }
    }
}
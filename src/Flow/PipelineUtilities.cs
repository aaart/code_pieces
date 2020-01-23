using System;
using System.Linq;

namespace Flow
{
    public static class PipelineUtilities
    {
        public static bool Execute<T>(this Func<IState<T>> method, out IState<T> state)
        {
            state = method();
            return !state.Errors.Any();
        }

        public static IState Decorate<T>(this Func<IState<T>> method, Func<IState<T>, IState> target) =>
            method.Execute(out IState<T> state) ? target(state) : state.Skip();

        public static IState<TR> Decorate<T, TR>(this Func<IState<T>> method, Func<IState<T>, IState<TR>> target) =>
            method.Execute(out IState<T> state) ? target(state) : state.Skip<TR>();

        public static IState<T> Decorate<T, TK>(this Func<IState<T>> method, Func<T, TK> transform, IFilter<TK> filter)
        {
            var state = method();
            try
            {
                TK target = transform(state.Result);
                if (!filter.Check(target, out IFilteringError error))
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

        public static T Sink<T, TState>(this TState state, Action<T, TState> setup = null)
            where T : PipelineResult, new()
            where TState : IState
        {
            var result = new T();
            result.Errors.AddRange(state.Errors);
            result.Exception = state.Exception;
            setup?.Invoke(result, state);
            state.Dispose();
            return result;
        }
    }
}
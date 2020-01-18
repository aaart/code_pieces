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

        public static IState Decorate<T>(this Func<IState<T>> method, Func<T, IState<T>, IState> target) =>
            method.Execute(out IState<T> state) ? target(state.Result, state) : state.Skip();

        public static IState<TR> Decorate<T, TR>(this Func<IState<T>> method, Func<T, IState<T>, IState<TR>> target) =>
            method.Execute(out IState<T> state) ? target(state.Result, state) : state.Skip<TR>();

        public static IState<T> Decorate<T, TK>(this Func<IState<T>> method, Func<T, TK> transform, IFilter<TK> filter)
        {
            var state = method();
            TK target = transform(state.Result);
            if (!filter.Check(target, out IError error))
            {
                state.PublishError(error);
            }
            return state;
        }
    }
}
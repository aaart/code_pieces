using System;
using System.Collections.Generic;
using System.Linq;

namespace Flow
{
    public class PipelineResult : IPipelineResult
    {
        internal static T CreateResult<T, TState>(TState state, Action<T, TState> setup = null)
            where T : PipelineResult, new()
            where TState : IState
        {
            var result = new T();
            result.Errors.AddRange(state.Errors);
            setup?.Invoke(result, state);
            state.EventReceiver.Dispose();
            return result;
        }

        public PipelineResult()
        {
            Errors = new List<IError>();
        }

        public List<IError> Errors { get; }

        IReadOnlyCollection<IError> IPipelineResult.Errors => Errors;

        public bool Failed => Errors.Any();
    }
    public class PipelineResult<T> : PipelineResult, IPipelineResult<T>
    {
        public T Result { get; set; }
    }
}
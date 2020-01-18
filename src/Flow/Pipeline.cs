using System;
using System.Linq;

namespace Flow
{
    public class Pipeline : IPipeline
    {
        private readonly Func<IState> _method;
        internal Pipeline(Func<IState> method)
        {
            _method = method;
        }

        public IPipelineResult Sink() => PipelineResult.CreateResult<PipelineResult, IState>(_method());

        
    }

    public class Pipeline<T> : IProjectablePipeline<T>
    {
        private readonly Func<IState<T>> _method;

        internal Pipeline(Func<IState<T>> method)
        {
            _method = method;
        }

        public IPipeline<TR> Project<TR>(Func<T, TR> projection) => 
            new Pipeline<TR>(() => _method.Decorate((argument, state) => state.Next(projection(state.Result))));

        public IPipelineResult<T> Sink() =>
            PipelineResult.CreateResult<PipelineResult<T>, IState<T>>(_method(), (result, state) =>
            {
                if (!state.Errors.Any())
                {
                    result.Value = state.Result;
                }
            });

        IPipelineResult IPipeline.Sink()
        {
            return Sink();
        }
    }
}
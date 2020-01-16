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

    public class Pipeline<T> : IPipeline<T>
    {
        private readonly Func<IState<T>> _method;
        
        internal Pipeline(Func<IState<T>> method)
        {
            _method = method;
        }

        public IPipelineResult<TR> Sink<TR>(Func<T, TR> projection) =>
            PipelineResult.CreateResult<PipelineResult<TR>, IState<T>>(_method(), (result, state) =>
            {
                if (!state.Errors.Any())
                {
                    result.Value = projection(state.Result);
                }
            });

        public IPipelineResult<T> Sink() => Sink(x => x);

        IPipelineResult IPipeline.Sink()
        {
            return Sink();
        }
    }
}
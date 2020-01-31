using System;
using System.Collections.Generic;
using System.Linq;

namespace Flow
{
    public class Pipeline<TFilteringError> : IPipeline<TFilteringError>
    {
        private readonly Func<IState<TFilteringError>> _method;
        internal Pipeline(Func<IState<TFilteringError>> method)
        {
            _method = method;
        }

        public (IPipelineResult, Exception, TFilteringError[]) Sink() => _method().Sink<PipelineResult, IState<TFilteringError>, TFilteringError>();
    }

    public class Pipeline<T, TFilteringError> : IProjectablePipeline<T, TFilteringError>
    {
        private readonly Func<IState<T, TFilteringError>> _method;

        internal Pipeline(Func<IState<T, TFilteringError>> method)
        {
            _method = method;
        }

        public IProjectablePipeline<TR, TFilteringError> Project<TR>(Func<T, TR> projection) => 
            new Pipeline<TR, TFilteringError>(() => _method.Decorate(state => projection(state.Result)));

        public (IPipelineResult<T>, Exception, TFilteringError[]) Sink() =>
            _method().Sink<PipelineResult<T>, IState<T, TFilteringError>, TFilteringError>((result, state) =>
            {
                if (!state.Invalid && !state.Broken)
                {
                    result.Value = state.Result;
                }
            });

        (IPipelineResult, Exception, TFilteringError[]) IPipeline<TFilteringError>.Sink()
        {
            return Sink();
        }
    }
}
using System;
using System.Linq;

namespace PipeSharp
{
    public class Pipeline<TFilteringError> : IPipeline<TFilteringError>
    {
        private readonly Func<IState<TFilteringError>> _method;
        internal Pipeline(Func<IState<TFilteringError>> method)
        {
            _method = method;
        }

        public IPipelineResult<TFilteringError> Sink() =>
            _method().Sink<PipelineResult<TFilteringError>, IState<TFilteringError>, TFilteringError>((result, state) =>
            {
                result.Exception = state.Exception;
                result.FilteringErrors = state.FilteringErrors.ToArray();
                result.Result = state.Invalid || state.Broken ? Result.FailedResult() : Result.SuccessResult();
            });
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

        public IPipelineResult<T, TFilteringError> Sink() =>
            _method().Sink<PipelineResult<T, TFilteringError>, IState<T, TFilteringError>, TFilteringError>((result, state) =>
            {
                result.Exception = state.Exception;
                result.FilteringErrors = state.FilteringErrors.ToArray();
                result.Result = state.Invalid || state.Broken ? Result<T>.FailedResult() : Result<T>.SuccessResult(state.Result);
            });

        IPipelineResult<TFilteringError> IPipeline<TFilteringError>.Sink()
        {
            return Sink();
        }
    }
}
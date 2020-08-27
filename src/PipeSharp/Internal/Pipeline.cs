using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace PipeSharp.Internal
{
    public class Pipeline<TError> : IPipeline<TError>
    {
        private readonly Func<IState<TError>> _method;
        private readonly Action<Exception, ILogger> _exceptionHandler;

        internal Pipeline(Func<IState<TError>> method, Action<Exception, ILogger> exceptionHandler)
        {
            _method = method;
            _exceptionHandler = exceptionHandler;
        }

        public IPipelineSummary<TError> Sink() =>
            _method().Sink<PipelineSummary<TError>, IState<TError>, TError>((result, state) =>
            {
                result.Exception = state.Exception;
                result.Errors = state.FilteringErrors.ToArray();
                result.Result = state.Invalid || state.Broken ? Result.FailedResult() : Result.SuccessResult();
            });
    }

    public class Pipeline<T, TError> : IProjectablePipeline<T, TError>
    {
        private readonly Func<IState<T, TError>> _method;
        private readonly Action<Exception, ILogger> _exceptionHandler;

        internal Pipeline(Func<IState<T, TError>> method, Action<Exception, ILogger> exceptionHandler)
        {
            _method = method;
            _exceptionHandler = exceptionHandler;
        }

        public IProjectablePipeline<TR, TError> Project<TR>(Func<T, TR> projection) =>
            new Pipeline<TR, TError>(() => _method.Decorate(state => projection(state.StepResult), () => { }, () => { }, _exceptionHandler), _exceptionHandler);

        public IPipelineSummary<T, TError> Sink() =>
            _method().Sink<PipelineSummary<T, TError>, IState<T, TError>, TError>((result, state) =>
            {
                result.Exception = state.Exception;
                result.Errors = state.FilteringErrors.ToArray();
                result.Result = state.Invalid || state.Broken ? Result<T>.FailedResult() : Result<T>.SuccessResult(state.StepResult);
            });

        IPipelineSummary<TError> IPipeline<TError>.Sink() => Sink();
    }
}
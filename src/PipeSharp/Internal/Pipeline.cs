using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace PipeSharp.Internal
{
    public class Pipeline<TError> : IPipeline<TError>
    {
        private readonly Func<IState<TError>> _method;
        private readonly Action<Exception, ILogger> _exceptionHandler;
        private readonly IEnumerable<Func<Exception, TError>> _exceptionToErrorMappers;
        
        internal Pipeline(Func<IState<TError>> method, Action<Exception, ILogger> exceptionHandler, IEnumerable<Func<Exception, TError>> exceptionToErrorMappers)
        {
            _method = method;
            _exceptionHandler = exceptionHandler;
            _exceptionToErrorMappers = exceptionToErrorMappers;
        }

        public IPipelineSummary<TError> Sink() =>
            _method().Sink<PipelineSummary<TError>, IState<TError>, TError>((summary, state) =>
            {
                summary.Exception = state.Exception;
                summary.Errors = state.FilteringErrors.ToArray();
                summary.Result = state.Invalid || state.Broken ? Result.FailedResult() : Result.SuccessResult();
                summary.ExceptionToErrorMappers = _exceptionToErrorMappers;
            });
    }

    public class Pipeline<T, TError> : IProjectablePipeline<T, TError>
    {
        private readonly Func<IState<T, TError>> _method;
        private readonly Action<Exception, ILogger> _exceptionHandler;
        private readonly IEnumerable<Func<Exception, TError>> _exceptionToErrorMappers;

        internal Pipeline(Func<IState<T, TError>> method, Action<Exception, ILogger> exceptionHandler, IEnumerable<Func<Exception, TError>> exceptionToErrorMappers)
        {
            _method = method;
            _exceptionHandler = exceptionHandler;
            _exceptionToErrorMappers = exceptionToErrorMappers;
        }

        public IProjectablePipeline<TR, TError> Project<TR>(Func<T, TR> projection) =>
            new Pipeline<TR, TError>(() => _method.Decorate(state => projection(state.StepResult), () => { }, () => { }, _exceptionHandler), _exceptionHandler, _exceptionToErrorMappers);

        public IPipelineSummary<T, TError> Sink() =>
            _method().Sink<PipelineSummary<T, TError>, IState<T, TError>, TError>((summary, state) =>
            {
                summary.Exception = state.Exception;
                summary.Errors = state.FilteringErrors.ToArray();
                summary.Result = state.Invalid || state.Broken ? Result<T>.FailedResult() : Result<T>.SuccessResult(state.StepResult);
                summary.ExceptionToErrorMappers = _exceptionToErrorMappers;
            });

        IPipelineSummary<TError> IPipeline<TError>.Sink() => Sink();
    }
}
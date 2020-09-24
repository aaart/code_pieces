using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using static PipeSharp.Internal.PipelineUtilities;

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
                summary.Errors = MergeExceptionAndErrors(state.Exception, _exceptionToErrorMappers, state.FilteringErrors);
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
                summary.Errors = MergeExceptionAndErrors(state.Exception, _exceptionToErrorMappers, state.FilteringErrors);
                summary.Value = state.StepResult;
            });

        IPipelineSummary<TError> IPipeline<TError>.Sink() => Sink();
    }
}
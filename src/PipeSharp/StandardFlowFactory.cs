using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PipeSharp.Internal;

namespace PipeSharp
{
    public class StandardFlowFactory<TFilteringError> : IFlowFactory<TFilteringError>
    {
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;

        public StandardFlowFactory()
        {
            _logger = NullLogger.Instance;
        }

        public StandardFlowFactory(ILogger logger)
        {
            _logger = logger;
        }

        public StandardFlowFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public IFlow<T, TFilteringError> For<T>(T target) => For(target, () => { }, () => { });
            

        public IFlow<T, TFilteringError> For<T>(T target, Action onDoing, Action onDone) =>
            new Step<T, TFilteringError>(
                () => new State<T, TFilteringError>(
                    target,
                    new StateData<TFilteringError>(
                        _logger ?? _loggerFactory.CreateLogger<Step<T, TFilteringError>>(),
                        new OutNullEventReceiver())),
                onDoing,
                onDone);
    }
}
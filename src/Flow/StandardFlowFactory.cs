using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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

        public IFlow<T, TFilteringError> For<T>(T target) => 
            new Step<T, TFilteringError>(
                () => new State<T, TFilteringError>(
                                target, 
                                new StateData<TFilteringError>(
                                    _logger ?? _loggerFactory.CreateLogger<Step<T, TFilteringError>>(), 
                                    new BlackholeEventReceiver())));
    }
}
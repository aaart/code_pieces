using Microsoft.Extensions.Logging;

namespace Flow
{
    public class StandardFlowFactory<TFilteringError> : IFlowFactory<TFilteringError>
    {
        private readonly ILoggerFactory _loggerFactory;

        public StandardFlowFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public IFlow<T, TFilteringError> For<T>(T target) => 
            new Step<T, TFilteringError>(
                () => new State<T, TFilteringError>(
                                target, 
                                new StateData<TFilteringError>(
                                    _loggerFactory.CreateLogger(typeof(Step<T, TFilteringError>)), 
                                    new BlackholeEventReceiver())));
    }
}
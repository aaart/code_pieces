using Microsoft.Extensions.Logging;

namespace Flow
{
    public class NotifyingFlowFactory<TFilteringError> : INotifyingFlowFactory<TFilteringError>
    {
        private readonly IEventReceiverFactory _eventReceiverFactory;
        private readonly ILoggerFactory _loggerFactory;

        public NotifyingFlowFactory(IEventReceiverFactory eventReceiverFactory, ILoggerFactory loggerFactory)
        {
            _eventReceiverFactory = eventReceiverFactory;
            _loggerFactory = loggerFactory;
        }

        public INotifyingFlow<T, TFilteringError> For<T>(T target) =>
            new Step<T, TFilteringError>(
                () => new State<T, TFilteringError>(
                    target, 
                    new StateData<TFilteringError>(
                            _loggerFactory.CreateLogger(typeof(Step<T, TFilteringError>)),
                            _eventReceiverFactory.Create())));
    }
}
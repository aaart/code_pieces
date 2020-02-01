namespace Flow
{
    public class NotifyingFlowFactory<TFilteringError> : INotifyingFlowFactory<TFilteringError>
    {
        private readonly IEventReceiverFactory _eventReceiverFactory;

        public NotifyingFlowFactory(IEventReceiverFactory eventReceiverFactory)
        {
            _eventReceiverFactory = eventReceiverFactory;
        }

        public INotifyingFlow<T, TFilteringError> For<T>(T target) =>
            new Step<T, TFilteringError>(() => new State<T, TFilteringError>(target, new StateData<TFilteringError>(_eventReceiverFactory.Create())));
    }
}
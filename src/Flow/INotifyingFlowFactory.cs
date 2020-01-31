namespace Flow
{
    public interface INotifyingFlowFactory<TFilteringError>
    {
        INotifyingFlow<T, TFilteringError> For<T>(T target);
    }
}
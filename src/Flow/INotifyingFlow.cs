namespace PipeSharp
{
    public interface INotifyingFlow<T, TFilteringError> : IFlow<T, TFilteringError>, IEventSource<T, TFilteringError>
    {
        
    }
}
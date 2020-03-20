namespace PipeSharp
{
    public interface INotifyingFlow<out T, TFilteringError> : IFlow<T, TFilteringError>, IEventSource<T, TFilteringError>
    {
        
    }
}
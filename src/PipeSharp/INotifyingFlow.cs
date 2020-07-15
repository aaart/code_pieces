namespace PipeSharp
{
    public interface INotifyingFlow<out T, TFilteringError> : IFlow<T, TFilteringError>, IEventPipelineNotifier<T, TFilteringError>
    {
        
    }
}
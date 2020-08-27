namespace PipeSharp
{
    public interface INotifyingFlow<out T, TError> : IFlow<T, TError>, IEventPipelineNotifier<T, TError>
    {
        
    }
}
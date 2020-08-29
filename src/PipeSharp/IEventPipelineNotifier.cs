using System;

namespace PipeSharp
{
    public interface IEventPipelineNotifier<out T, TError>
    {
        ICheckedAndCheckable<T, TError> Raise<TEvent>(Func<T, TEvent> func);
    }
}
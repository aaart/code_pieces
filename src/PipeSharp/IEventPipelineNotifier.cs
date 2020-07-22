using System;

namespace PipeSharp
{
    public interface IEventPipelineNotifier<out T, TFilteringError>
    {
        ICheckedAndCheckable<T, TFilteringError> Raise<TEvent>(Func<T, TEvent> func);
    }
}
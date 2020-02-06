using System;

namespace PipeSharp
{
    public interface IEventSource<out T, TFilteringError>
    {
        ICheckedAndCheckable<T, TFilteringError> Raise<TEvent>(Func<T, TEvent> func);
    }
}
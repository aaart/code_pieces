using System;

namespace PipeSharp
{
    public interface IEventSource<out T, TFilteringError>
    {
        IValidatedVerified<T, TFilteringError> Raise<TEvent>(Func<T, TEvent> func);
    }
}
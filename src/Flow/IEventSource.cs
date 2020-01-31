using System;

namespace Flow
{
    public interface IEventSource<out T, TFilteringError>
    {
        IValidatedVerified<T, TFilteringError> Raise(Func<T, IEvent> func);
    }
}
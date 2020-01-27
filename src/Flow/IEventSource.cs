using System;

namespace Flow
{
    public interface IEventSource<out T>
    {
        IValidatedVerified<T> Raise(Action<T, IEventReceiver> publishingMethod);
    }
}
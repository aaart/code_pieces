using System;

namespace Flow
{
    public interface IEventPublisher<out T>
    {
        IValidatedVerified<T> Publish(Action<T, IEventReceiver> publishingMethod);
    }
}
using System;

namespace Flow
{
    public interface IEventPublisher<out T>
    {
        IValidatedVerified<T> Publish<TE>(Func<T, TE> publishEvent)
            where TE : IEvent;
    }
}
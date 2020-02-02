using System;

namespace PipeSharp
{
    public abstract class ImmediatePublishEventReceiver : IEventReceiver
    {
        public void Receive<TEvent>(TEvent e)
        {
            CreatePublisher(e).Invoke();
        }

        protected abstract Action CreatePublisher<TEvent>(TEvent e);

        public void Dispose()
        {
        }
    }
}
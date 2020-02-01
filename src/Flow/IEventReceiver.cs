using System;

namespace Flow
{
    public interface IEventReceiver : IDisposable
    {
        void Receive<TEvent>(TEvent e);
    }
}
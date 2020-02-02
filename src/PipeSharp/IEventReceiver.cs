using System;

namespace PipeSharp
{
    public interface IEventReceiver : IDisposable
    {
        void Receive<TEvent>(TEvent e);
    }
}
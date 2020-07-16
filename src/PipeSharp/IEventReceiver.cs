using System;

namespace PipeSharp
{
    public interface IEventReceiver<in TEvent> : IDisposable
    {
        void Receive(TEvent e);
    }
}
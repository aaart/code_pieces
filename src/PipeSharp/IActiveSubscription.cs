using System;

namespace PipeSharp
{
    public interface IActiveSubscription : IDisposable
    {
        void Receive<TEvent>(TEvent e);
    }
}
using System;

namespace Flow
{
    public interface IEventReceiver : IDisposable
    {
        void Receive<TE>(TE e)
            where TE : IEvent;
    }
}
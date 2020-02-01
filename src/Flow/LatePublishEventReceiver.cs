using System;
using System.Collections.Generic;

namespace Flow
{
    public abstract class LatePublishEventReceiver : IEventReceiver
    {
        private readonly List<IEvent> _events = new List<IEvent>();

        public abstract void Receive<TE>(TE e) where TE : IEvent;

        protected abstract void PublishAll(IEnumerable<IEvent> events);

        public void Dispose()
        {
            PublishAll(_events);
        }
    }
}
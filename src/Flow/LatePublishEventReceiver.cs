using System;
using System.Collections.Generic;

namespace Flow
{
    public abstract class LatePublishEventReceiver : IEventReceiver
    {
        private readonly List<Action> _publishers = new List<Action>();

        public abstract void Receive<TEvent>(TEvent e);

        protected void PublishAll(List<Action> publishers) => publishers.ForEach(publisher => publisher());

        public void Dispose() => PublishAll(_publishers);
    }
}
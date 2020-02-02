using System;
using System.Collections.Generic;

namespace PipeSharp
{
    public abstract class LatePublishEventReceiver : IEventReceiver
    {
        private readonly List<Action> _publishers = new List<Action>();

        public void Receive<TEvent>(TEvent e) => _publishers.Add(CreatePublisher(e));

        protected abstract Action CreatePublisher<TEvent>(TEvent e);

        protected void PublishAll(List<Action> publishers) => publishers.ForEach(publisher => publisher());

        public void Dispose() => PublishAll(_publishers);
    }
}
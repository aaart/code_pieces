namespace PipeSharp
{
    public abstract class AbstractActiveSubscription : IActiveSubscription
    {
        public abstract void Dispose();

        public void Receive<TEvent>(TEvent e)
        {
            IEventReceiver<TEvent> eventReceiver = GetReceiver<TEvent>();
            eventReceiver.Receive(e);
        }

        protected abstract IEventReceiver<TEvent> GetReceiver<TEvent>();
    }
}
namespace PipeSharp
{
    public abstract class AbstractEventReceiver<TEvent> : IEventReceiver<TEvent>
    {
        public abstract void Dispose();
        public abstract void Receive(TEvent e);
    }
}
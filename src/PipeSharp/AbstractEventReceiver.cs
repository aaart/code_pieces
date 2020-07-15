namespace PipeSharp
{
    public abstract class AbstractEventReceiver : IEventReceiver
    {
        public abstract void Dispose();
        public abstract void Receive<TEvent>(TEvent e);
    }
}
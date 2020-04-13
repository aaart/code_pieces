namespace PipeSharp.Internal
{
    public class OutNullEventReceiver : IEventReceiver
    {
        public void Receive<TEvent>(TEvent e) 
        {
        }

        public void Dispose()
        {
        }
    }
}
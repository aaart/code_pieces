namespace PipeSharp
{
    public class BlackholeEventReceiver : IEventReceiver
    {
        public void Receive<TEvent>(TEvent e) 
        {
        }

        public void Dispose()
        {
        }
    }
}
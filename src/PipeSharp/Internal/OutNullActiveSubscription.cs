namespace PipeSharp.Internal
{
    public class OutNullActiveSubscription : IActiveSubscription
    {
        public void Dispose()
        {
        }

        public void Receive<TEvent>(TEvent e)
        {
        }
    }
}
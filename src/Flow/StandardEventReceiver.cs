namespace Flow
{
    public class StandardEventReceiver : IEventReceiver
    {
        public void Receive<TE>(TE e) where TE : IEvent
        {
        }
    }
}
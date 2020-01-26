namespace Flow
{
    public class BlackholeEventReceiver : IEventReceiver
    {
        public void Receive<TE>(TE e) 
            where TE : IEvent
        {
        }
    }
}
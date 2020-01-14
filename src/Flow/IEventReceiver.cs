namespace Flow
{
    public interface IEventReceiver
    {
        void Receive<TE>(TE e)
            where TE : IEvent;
    }
}
namespace Flow
{
    public interface IEventReceiverFactory
    {
        IEventReceiver Create();
    }
}
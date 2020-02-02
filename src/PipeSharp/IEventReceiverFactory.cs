namespace PipeSharp
{
    public interface IEventReceiverFactory
    {
        IEventReceiver Create();
    }
}
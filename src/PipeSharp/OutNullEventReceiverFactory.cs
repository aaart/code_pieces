namespace PipeSharp
{
    public class OutNullEventReceiverFactory : IEventReceiverFactory
    {
        public IEventReceiver Create() => new OutNullEventReceiver();
    }
}
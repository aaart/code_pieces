namespace PipeSharp.Internal
{
    public class OutNullEventReceiverFactory : IEventReceiverFactory
    {
        public IEventReceiver Create() => new OutNullEventReceiver();
    }
}
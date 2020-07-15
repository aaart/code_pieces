namespace PipeSharp.Internal
{
    public class OutNullSubscription : ISubscription
    {
        public IEventReceiver Subscribe() => new OutNullEventReceiver();
    }
}
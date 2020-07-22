namespace PipeSharp.Internal
{
    public class OutNullSubscription : ISubscription
    {
        public IActiveSubscription Subscribe() => new OutNullActiveSubscription();
    }
}
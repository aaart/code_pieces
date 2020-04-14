namespace PipeSharp
{
    public class GenericEventReceiverFactory<T> : IEventReceiverFactory
        where T : IEventReceiver, new()
    {
        public IEventReceiver Create() => new T();
    }
}
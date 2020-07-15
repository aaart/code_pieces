namespace PipeSharp
{
    public class GenericSubscription<T> : ISubscription
        where T : IEventReceiver, new()
    {
        public IEventReceiver Subscribe() => new T();
    }
}
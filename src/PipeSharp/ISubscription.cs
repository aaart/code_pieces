namespace PipeSharp
{
    public interface ISubscription
    {
        IEventReceiver Subscribe();
    }
}
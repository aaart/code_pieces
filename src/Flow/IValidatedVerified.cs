namespace Flow
{
    public interface IValidatedVerified<out T> : IValidated<T>, IVerificationReady<T>, IEventPublisher<T>
    {
        
    }
}
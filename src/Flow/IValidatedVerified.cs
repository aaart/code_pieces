namespace Flow
{
    public interface IValidatedVerified<T> : IValidated<T>, IVerificationReady<T>
    {
        
    }
}
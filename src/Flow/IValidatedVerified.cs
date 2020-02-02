namespace PipeSharp
{
    public interface IValidatedVerified<out T, TFilteringError> : IValidated<T, TFilteringError>, IVerificationReady<T, TFilteringError>
    { 
    }
}
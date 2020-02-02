namespace PipeSharp
{
    public interface IFlow<T, TFilteringError> : IValidationReady<T, TFilteringError>, IValidated<T, TFilteringError>
    {
        
    }
}
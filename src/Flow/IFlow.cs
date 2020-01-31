namespace Flow
{
    public interface IFlow<T, TFilteringError> : IValidationReady<T, TFilteringError>, IValidated<T, TFilteringError>
    {
        
    }
}
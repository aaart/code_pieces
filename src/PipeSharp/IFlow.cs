namespace PipeSharp
{
    public interface IFlow<out T, TFilteringError> : ICheckable<T, TFilteringError>, IChecked<T, TFilteringError>
    {
        
    }
}
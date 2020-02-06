namespace PipeSharp
{
    public interface ICheckedAndCheckable<out T, TFilteringError> : IChecked<T, TFilteringError>, ICheckable<T, TFilteringError>
    { 
    }
}
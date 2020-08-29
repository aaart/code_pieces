namespace PipeSharp
{
    public interface ICheckedAndCheckable<out T, TError> : IChecked<T, TError>, ICheckable<T, TError>
    { 
    }
}
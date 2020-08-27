namespace PipeSharp
{
    public interface IFlow<out T, TError> : ICheckable<T, TError>, IChecked<T, TError>
    {
        
    }
}
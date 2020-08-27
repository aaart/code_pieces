namespace PipeSharp
{
    public interface IFilter<in T, TError>
    {
        bool Check(T target, out TError filteringError);
    }
}
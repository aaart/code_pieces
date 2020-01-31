namespace Flow
{
    public interface IFilter<in T, TFilteringError>
    {
        bool Check(T target, out TFilteringError filteringError);
    }
}
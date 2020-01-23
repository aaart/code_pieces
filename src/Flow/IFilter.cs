namespace Flow
{
    public interface IFilter<in T>
    {
        bool Check(T target, out IFilteringError filteringError);
    }
}
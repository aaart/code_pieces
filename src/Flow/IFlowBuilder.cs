namespace Flow
{
    public interface IFlowBuilder<TFilteringError>
    {
        IFlow<T, TFilteringError> For<T>(T target);
    }
}
namespace PipeSharp
{
    public interface IFlowFactory<TFilteringError>
    {
        IFlow<T, TFilteringError> For<T>(T target);
    }
}
namespace Flow
{
    public interface IFlowBuilder
    {
        IFlow<T> For<T>(T target);
    }
}
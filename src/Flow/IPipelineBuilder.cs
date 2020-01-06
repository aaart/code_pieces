namespace Flow
{
    public interface IPipelineBuilder
    {
        IBeginFlow<T> For<T>(T target);
    }
}
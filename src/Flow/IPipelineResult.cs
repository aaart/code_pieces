namespace PipeSharp
{
    public interface IPipelineResult
    {
        bool Failed { get; }
    }

    public interface IPipelineResult<out T> : IPipelineResult
    {
        T Value { get; }
    }
}
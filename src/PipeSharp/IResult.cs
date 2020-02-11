namespace PipeSharp
{
    public interface IResult
    {
        bool Failed { get; }
    }

    public interface IResult<out T> : IResult
    {
        T Value { get; }
    }
}
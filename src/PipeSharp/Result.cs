namespace PipeSharp
{
    public class Result : IResult
    {
       public bool Failed { get; set; }
    }
    public class Result<T> : Result, IResult<T>
    {
        public T Value { get; set; }
    }
}
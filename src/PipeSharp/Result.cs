namespace PipeSharp
{
    public class Result : IResult
    {
        public static Result FailedResult() => new Result { Failed = true };
        public static Result SuccessResult() => new Result { Failed = false };

        protected Result()
        {   
        }

        public bool Failed { get; protected set; }
    }
    public class Result<T> : Result, IResult<T>
    {
        public new static Result<T> FailedResult() => new Result<T> { Failed = true };
        public static Result<T> SuccessResult(T val) => new Result<T> { Failed = false, Value = val};

        protected Result()
        {
        }

        public T Value { get; protected set; }
    }
}
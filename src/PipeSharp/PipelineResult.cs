using System;

namespace PipeSharp
{
    public class PipelineResult<TFilteringError> : IPipelineResult<TFilteringError>
    {
        public IResult Result { get; set; }
        public Exception Exception { get; set; }
        public TFilteringError[] FilteringErrors { get; set; }
        public void Deconstruct(out IResult result, out Exception exception, out TFilteringError[] errors)
        {
            result = Result;
            exception = Exception;
            errors = FilteringErrors;
        }
    }

    public class PipelineResult<T, TFilteringError> : PipelineResult<TFilteringError>, IPipelineResult<T, TFilteringError>
    {
        public new IResult<T> Result { get; set; }
        public void Deconstruct(out IResult<T> result, out Exception exception, out TFilteringError[] errors)
        {
            result = Result;
            exception = Exception;
            errors = FilteringErrors;
        }
    }
}
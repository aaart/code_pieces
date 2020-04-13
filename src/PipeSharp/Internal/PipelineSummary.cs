using System;

namespace PipeSharp.Internal
{
    public class PipelineSummary<TFilteringError> : IPipelineSummary<TFilteringError>
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

    public class PipelineSummary<T, TFilteringError> : PipelineSummary<TFilteringError>, IPipelineSummary<T, TFilteringError>
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace PipeSharp.Internal
{
    public class PipelineSummary<TError> : IPipelineSummary<TError>
    {
        public IResult Result { get; set; }
        public Exception Exception { get; set; }
        public TError[] Errors { get; set; }
        public IEnumerable<Func<Exception, TError>> ExceptionToErrorMappers { get; set; }
        public void Deconstruct(out IResult result, out TError[] errors)
        {
            result = Result;
            errors = Exception != null ? Errors.Union(ExceptionToErrorMappers.Select(m => m(Exception))).ToArray() : Errors;
        }

        public void Deconstruct(out IResult result, out Exception exception, out TError[] errors)
        {
            result = Result;
            exception = Exception;
            errors = Errors;
        }
    }

    public class PipelineSummary<T, TError> : PipelineSummary<TError>, IPipelineSummary<T, TError>
    {
        public new IResult<T> Result { get; set; }
        public void Deconstruct(out IResult<T> result, out TError[] errors)
        {
            result = Result;
            errors = Exception != null ? Errors.Union(ExceptionToErrorMappers.Select(m => m(Exception))).ToArray() : Errors;
        }

        public void Deconstruct(out IResult<T> result, out Exception exception, out TError[] errors)
        {
            result = Result;
            exception = Exception;
            errors = Errors;
        }
    }
}
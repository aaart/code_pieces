using System;
using System.Collections.Generic;
using System.Linq;

namespace PipeSharp.Internal
{
    public class PipelineSummary<TError> : IPipelineSummary<TError>
    {
        public bool Failed { get; set; }
        public Exception Exception { get; set; }
        public TError[] Errors { get; set; }
        public IEnumerable<Func<Exception, TError>> ExceptionToErrorMappers { get; set; }
        public void Deconstruct(out bool failed, out TError[] errors)
        {
            failed = Failed;
            errors = Exception != null ? Errors.Union(ExceptionToErrorMappers.Select(m => m(Exception))).ToArray() : Errors;
        }

        public void Deconstruct(out bool failed, out Exception exception, out TError[] errors)
        {
            failed = Failed;
            exception = Exception;
            errors = Errors;
        }
    }

    public class PipelineSummary<T, TError> : PipelineSummary<TError>, IPipelineSummary<T, TError>
    {
        public T Value { get; set; }

        public void Deconstruct(out bool failed, out T value, out TError[] errors)
        {
            failed = Failed;
            value = Value;
            errors = Exception != null ? Errors.Union(ExceptionToErrorMappers.Select(m => m(Exception))).ToArray() : Errors;
        }

        public void Deconstruct(out bool failed, out T value, out Exception exception, out TError[] errors)
        {
            failed = Failed;
            value = Value;
            exception = Exception;
            errors = Errors;
        }
    }
}
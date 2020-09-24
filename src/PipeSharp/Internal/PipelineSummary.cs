using System;
using System.Collections.Generic;
using System.Linq;

namespace PipeSharp.Internal
{
    public class PipelineSummary<TError> : IPipelineSummary<TError>
    {
        public TError[] Errors { get; set; }
        
        public void Deconstruct(out TError[] errors)
        {
            errors = Errors;
        }
    }

    public class PipelineSummary<T, TError> : PipelineSummary<TError>, IPipelineSummary<T, TError>
    {
        public T Value { get; set; }

        public void Deconstruct(out T value, out TError[] errors)
        {
            value = Value;
            errors = Errors;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Flow
{
    public class PipelineResult : IPipelineResult
    {
        public PipelineResult()
        {
            Errors = new List<IError>();
        }

        public List<IError> Errors { get; }

        IReadOnlyCollection<IError> IPipelineResult.Errors => Errors;

        public bool Failed => Errors.Any();
    }
    public class PipelineResult<T> : PipelineResult, IPipelineResult<T>
    {
        public T Value { get; set; }
    }
}
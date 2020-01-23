﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Flow
{
    public class PipelineResult : IPipelineResult
    {
        public PipelineResult()
        {
            Errors = new List<IFilteringError>();
        }

        public List<IFilteringError> Errors { get; }

        IReadOnlyCollection<IFilteringError> IPipelineResult.FilteringErrors => Errors;
        public Exception Exception { get; }

        public bool Failed => Exception != null || Errors.Any();
    }
    public class PipelineResult<T> : PipelineResult, IPipelineResult<T>
    {
        public T Value { get; set; }
    }
}
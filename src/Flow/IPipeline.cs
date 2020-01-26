using System;
using System.Collections.Generic;

namespace Flow
{
    public interface IPipeline
    {
        (IPipelineResult, Exception, IFilteringError[]) Sink();
    }

    public interface IPipeline<T> : IPipeline
    {
        new (IPipelineResult<T>, Exception, IFilteringError[]) Sink();
    }
}
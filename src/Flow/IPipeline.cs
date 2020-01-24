using System;
using System.Collections.Generic;

namespace Flow
{
    public interface IPipeline
    {
        (IPipelineResult, Exception, IReadOnlyCollection<IFilteringError>) Sink();
    }

    public interface IPipeline<T> : IPipeline
    {
        new (IPipelineResult<T>, Exception, IReadOnlyCollection<IFilteringError>) Sink();
    }
}
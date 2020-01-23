using System;
using System.Collections.Generic;

namespace Flow
{
    public interface IPipelineResult
    {
        IReadOnlyCollection<IFilteringError> FilteringErrors { get; }
        Exception Exception { get; }
        bool Failed { get; }
    }

    public interface IPipelineResult<out T> : IPipelineResult
    {
        T Value { get; }
    }
}
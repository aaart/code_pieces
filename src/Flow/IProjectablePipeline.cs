using System;

namespace Flow
{
    public interface IProjectablePipeline<T, TFilteringError> : IPipeline<T, TFilteringError>
    {
        IProjectablePipeline<TR, TFilteringError> Project<TR>(Func<T, TR> projection);
    }
}
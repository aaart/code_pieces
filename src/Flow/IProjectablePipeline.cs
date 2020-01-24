using System;

namespace Flow
{
    public interface IProjectablePipeline<T> : IPipeline<T>
    {
        IProjectablePipeline<TR> Project<TR>(Func<T, TR> projection);
    }
}
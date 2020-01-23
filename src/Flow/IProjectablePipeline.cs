using System;

namespace Flow
{
    public interface IProjectablePipeline<out T> : IPipeline<T>
    {
        IProjectablePipeline<TR> Project<TR>(Func<T, TR> projection);
    }
}
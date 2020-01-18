using System;

namespace Flow
{
    public interface IProjectablePipeline<out T> : IPipeline<T>
    {
        IPipeline<TR> Project<TR>(Func<T, TR> projection);
    }
}
using System;

namespace PipeSharp
{
    public interface IProjectablePipeline<T, TError> : IPipeline<T, TError>
    {
        IProjectablePipeline<TR, TError> Project<TR>(Func<T, TR> projection);
    }
}
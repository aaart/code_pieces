using System.Collections.Generic;

namespace Flow
{
    public interface IPipelineResult
    {
        IReadOnlyCollection<IError> Errors { get; }
        bool Failed { get; }
    }

    public interface IPipelineResult<out T> : IPipelineResult
    {
        T Value { get; }
    }
}
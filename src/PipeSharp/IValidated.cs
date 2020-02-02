using System;

namespace PipeSharp
{
    public interface IValidated<out T, TFilteringError>
    {
        IValidatedVerified<TR, TFilteringError> Apply<TR>(Func<T, TR> apply);
        IPipeline<TFilteringError> Finalize(Action<T> execution);
        IProjectablePipeline<TR, TFilteringError> Finalize<TR>(Func<T, TR> execution);
    }
}
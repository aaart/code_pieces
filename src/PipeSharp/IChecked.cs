using System;

namespace PipeSharp
{
    public interface IChecked<out T, TFilteringError>
    {
        ICheckedAndCheckable<TR, TFilteringError> Apply<TR>(Func<T, TR> apply);
        IPipeline<TFilteringError> Finalize(Action<T> execution);
        IProjectablePipeline<TR, TFilteringError> Finalize<TR>(Func<T, TR> execution);
    }
}
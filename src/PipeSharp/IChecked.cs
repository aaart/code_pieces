using System;

namespace PipeSharp
{
    public interface IChecked<out T, TError>
    {
        ICheckedAndCheckable<TR, TError> Apply<TR>(Func<T, TR> apply);
        IPipeline<TError> Finalize(Action<T> execution);
        IProjectablePipeline<TR, TError> Finalize<TR>(Func<T, TR> execution);
    }
}
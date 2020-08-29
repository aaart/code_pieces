using System;

namespace PipeSharp
{
    public interface ICheckable<out T, TError>
    {
        IFlow<T, TError> Check<TR>(Func<T, TR> transform, Func<TR, bool> validator, Func<TError> error);
        IFlow<T, TError> Check(Func<T, bool> validator, Func<TError> error);
        IFlow<T, TError> Check<TR>(Func<T, TR> transform, IFilter<TR, TError> filter);
        IFlow<T, TError> Check(IFilter<T, TError> filter);
    }
}
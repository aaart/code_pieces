using System;

namespace PipeSharp
{
    public interface ICheckable<out T, TFilteringError>
    {
        IFlow<T, TFilteringError> Check<TR>(Func<T, TR> transform, Func<TR, bool> validator, Func<TFilteringError> error);
        IFlow<T, TFilteringError> Check(Func<T, bool> validator, Func<TFilteringError> error);
        IFlow<T, TFilteringError> Check<TR>(Func<T, TR> transform, IFilter<TR, TFilteringError> filter);
        IFlow<T, TFilteringError> Check(IFilter<T, TFilteringError> filter);
    }
}
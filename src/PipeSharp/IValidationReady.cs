using System;

namespace PipeSharp
{
    public interface IValidationReady<T, TFilteringError>
    {
        IFlow<T, TFilteringError> Validate<TR>(Func<T, TR> transform, Func<TR, bool> validator, Func<TFilteringError> error);
        IFlow<T, TFilteringError> Validate(Func<T, bool> validator, Func<TFilteringError> error);
        IFlow<T, TFilteringError> Validate<TR>(Func<T, TR> transform, IFilter<TR, TFilteringError> filter);
        IFlow<T, TFilteringError> Validate(IFilter<T, TFilteringError> filter);
    }
}
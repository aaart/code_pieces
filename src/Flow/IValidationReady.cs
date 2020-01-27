using System;
using System.Linq.Expressions;

namespace Flow
{
    public interface IValidationReady<T>
    {
        IFlow<T> Validate<TR>(Func<T, TR> transform, Func<TR, bool> validator, Func<IFilteringError> error);
        IFlow<T> Validate(Func<T, bool> validator, Func<IFilteringError> error);
        IFlow<T> Validate<TR>(Func<T, TR> transform, IFilter<TR> filter);
        IFlow<T> Validate(IFilter<T> filter);
    }
}
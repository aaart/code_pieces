using System;
using System.Linq.Expressions;

namespace Flow
{
    public interface IValidationReady<T>
    {
        IBeginFlow<T> Validate<TR>(Func<T, TR> transform, Func<TR, bool> validator, Func<IError> error);
        IBeginFlow<T> Validate(Func<T, bool> validator, Func<IError> error);
        IBeginFlow<T> Validate<TR>(Func<T, TR> transform, IFilter<TR> filter);
        IBeginFlow<T> Validate(IFilter<T> filter);
    }
}
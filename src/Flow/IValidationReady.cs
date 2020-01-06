using System;
using System.Linq.Expressions;

namespace Flow
{
    public interface IValidationReady<T>
    {
        IBeginFlow<T> Validate<TR>(Func<T, TR> validationTarget, Func<TR, bool> validator, Func<IError> error);
        IBeginFlow<T> Validate(Func<T, bool> validator, Func<IError> error);
        IBeginFlow<T> Validate<TR>(Func<T, TR> validationTarget, IFilter<TR> filter);
        IBeginFlow<T> Validate(IFilter<T> filter);
    }
}
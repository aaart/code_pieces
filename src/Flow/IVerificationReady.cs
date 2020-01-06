using System;
using System.Linq.Expressions;

namespace Flow
{
    public interface IVerificationReady<out T>
    {
        IValidated<T> Verify<TR>(Func<T, TR> verificationTarget, Func<TR, bool> check, Func<IError> error);
        IValidated<T> Verify(Func<T, bool> check, Func<IError> error);
        IValidated<T> Verify(IFilter<T> filter);
        IValidated<T> Verify<TR>(Func<T, TR> verificationTarget, IFilter<TR> filter);
    }
}
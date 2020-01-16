using System;

namespace Flow
{
    public interface IVerificationReady<out T>
    {
        IValidatedVerified<T> Verify<TR>(Func<T, TR> transform, Func<TR, bool> check, Func<IError> error);
        IValidatedVerified<T> Verify(Func<T, bool> check, Func<IError> error);
        IValidatedVerified<T> Verify(IFilter<T> filter);
        IValidatedVerified<T> Verify<TR>(Func<T, TR> transform, IFilter<TR> filter);
    }
}
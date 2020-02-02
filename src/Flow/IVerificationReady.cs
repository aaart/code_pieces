using System;

namespace PipeSharp
{
    public interface IVerificationReady<out T, TFilteringError>
    {
        IValidatedVerified<T, TFilteringError> Verify<TR>(Func<T, TR> transform, Func<TR, bool> check, Func<TFilteringError> error);
        IValidatedVerified<T, TFilteringError> Verify(Func<T, bool> check, Func<TFilteringError> error);
        IValidatedVerified<T, TFilteringError> Verify(IFilter<T, TFilteringError> filter);
        IValidatedVerified<T, TFilteringError> Verify<TR>(Func<T, TR> transform, IFilter<TR, TFilteringError> filter);
    }
}
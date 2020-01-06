using System;

namespace Flow
{
    public interface IValidated<out T>
    {
        IValidatedVerified<TR> Apply<TR>(Func<T, TR> apply);
        IPipeline Finalize(Action<T> execution);
        IPipeline<TR> Finalize<TR>(Func<T, TR> execution);
    }
}
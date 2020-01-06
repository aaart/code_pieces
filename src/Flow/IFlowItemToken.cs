using System.Collections.Generic;

namespace Flow
{
    public interface IFlowItemToken
    {
        object CurrentResult { get; }
        void PushResult(object o);

        IError CurrentError { get; }

        void PushError(IError error);

        IEnumerable<IError> Errors { get; }
    }
}
using System.Collections.Generic;

namespace Flow
{
    public interface IFlowItemState
    {
        T CurrentResult<T>();
        void PushResult(object o);

        IError CurrentError { get; }

        void PushError(IError error);

        IEnumerable<IError> Errors { get; }

        IEventReceiver EventReceiver { get; }
    }
}
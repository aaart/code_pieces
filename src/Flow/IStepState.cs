using System;
using System.Collections.Generic;

namespace Flow
{

    public interface IStepState
    {
        bool Failed { get; }
        IStepState Skip();
        IStepState Fail();
        void PublishFilteringError(IFilteringError filteringError);
        void PublishException(Exception exception);
        IEnumerable<IFilteringError> FilteringErrors { get; }
        Exception Exception { get; }
        IEventReceiver EventReceiver { get; }
        void Done();
    }
}
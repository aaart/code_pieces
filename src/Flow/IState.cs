using System;
using System.Collections.Generic;

namespace Flow
{

    public interface IState
    {
        bool Failed { get; }
        IState Skip();
        IState Fail();
        void PublishFilteringError(IFilteringError filteringError);
        void PublishException(Exception exception);
        IEnumerable<IFilteringError> FilteringErrors { get; }
        Exception Exception { get; }
        IEventReceiver EventReceiver { get; }
        void Done();
    }

    public interface IState<out T> : IState
    {
        IState Next();
        IState<TR> Next<TR>(TR result);
        IState<TR> Skip<TR>();
        IState<TR> Fail<TR>();

        T Result { get; }
    }
}
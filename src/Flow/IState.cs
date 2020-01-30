using System;
using System.Collections.Generic;

namespace Flow
{

    public interface IState
    {
        bool Failed { get; }
        bool Invalid { get; }
        IState Fail();
        IState Fail(Exception exception);
        IState Fail(IFilteringError filteringError);
        IEnumerable<IFilteringError> FilteringErrors { get; }
        Exception Exception { get; }
        IEventReceiver EventReceiver { get; }
        void Done();
    }

    public interface IState<out T> : IState
    {
        IState Next();
        IState<TR> Next<TR>(TR result);
        IState<TR> Fail<TR>();
        IState<TR> Fail<TR>(Exception exception);
        new IState<T> Fail(IFilteringError filteringError);
        T Result { get; }
    }
}
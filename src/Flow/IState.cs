using System;
using System.Collections.Generic;

namespace Flow
{

    public interface IState
    {
        bool Failed { get; }
        IState Skip();
        IState Fail();
        void PublishError(IFilteringError filteringError);
        IEnumerable<IFilteringError> Errors { get; }
        Exception Exception { get; set; }
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
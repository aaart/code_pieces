using System;
using System.Collections.Generic;

namespace Flow
{

    public interface IState
    {
        IState Skip();
        void PublishError(IFilteringError filteringError);
        IEnumerable<IFilteringError> Errors { get; }
        Exception Exception { get; set; }
        IEventReceiver EventReceiver { get; }
        void Finalize();
    }

    public interface IState<out T> : IState
    {
        IState Next();
        IState<TR> Next<TR>(TR result);
        new IState<TR> Skip<TR>();
        
        T Result { get; }
    }
}
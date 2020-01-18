using System.Collections.Generic;

namespace Flow
{

    public interface IState
    {
        IState Skip();
        void PublishError(IError error);
        IEnumerable<IError> Errors { get; }
        void Receive(IEvent @event);
    }

    public interface IState<out T> : IState
    {
        IState Next();
        IState<TR> Next<TR>(TR result);
        new IState<TR> Skip<TR>();
        
        T Result { get; }
    }
}
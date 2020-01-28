using System;
using System.Collections.Generic;

namespace Flow
{
    public class State : IState
    {
        protected internal State(StateData stateData)
        {
            Data = stateData;
        }
        protected StateData Data { get; }

        IEventReceiver IState.EventReceiver => Data.EventReceiver;
        public IEnumerable<IFilteringError> FilteringErrors => Data.FilteringErrors;
        public void PublishException(Exception exception) => Data.Exception = exception;
        public Exception Exception => Data.Exception;
        public bool Failed => Data.Failed;

        public IState Skip() => this;
        public IState Fail()
        {
            Data.Failed = true;
            return this;
        }

        public void PublishFilteringError(IFilteringError filteringError) => Data.FilteringErrors.Add(filteringError);

        public void Done() => Data.Dispose();
    }

    public class State<T> : State, IState<T>
    {
        public State(T result, StateData stateData)
            : base(stateData)
        {
            Result = result;
        }

        public T Result { get; }

        public IState<TR> Next<TR>(TR result) => new State<TR>(result, Data);

        public IState<TR> Skip<TR>() => new State<TR>(default, Data);

        public IState<TR> Fail<TR>()
        {
            Data.Failed = true;
            return new State<TR>(default, Data);
        }

        public IState Next() => new State(Data);
    }
}
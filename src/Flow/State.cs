using System;
using System.Collections.Generic;

namespace Flow
{
    public class State<TEventReceiver> : IState
        where TEventReceiver : IEventReceiver
    {
        protected internal State(StateData<TEventReceiver> stateData)
        {
            Data = stateData;
        }
        protected StateData<TEventReceiver> Data { get; }

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

        public void Done() => Data.OnStateDone(Data.FilteringErrors, Data.EventReceiver);
    }

    public class State<T, TEventReceiver> : State<TEventReceiver>, IState<T>
        where TEventReceiver : IEventReceiver
    {
        public State(T result, StateData<TEventReceiver> stateData)
            : base(stateData)
        {
            Result = result;
        }

        public T Result { get; }

        public IState<TR> Next<TR>(TR result) => new State<TR, TEventReceiver>(result, Data);

        public IState<TR> Skip<TR>() => new State<TR, TEventReceiver>(default, Data);

        public IState<TR> Fail<TR>()
        {
            Data.Failed = true;
            return new State<TR, TEventReceiver>(default, Data);
        }

        public IState Next() => new State<TEventReceiver>(Data);
    }
}
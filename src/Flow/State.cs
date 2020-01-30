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
        public Exception Exception => Data.Exception;
        public bool Failed => Data.Failed;
        public bool Invalid => Data.Invalid;

        public IState Fail()
        {
            Data.Failed = true;
            return this;
        }

        public IState Fail(Exception exception)
        {
            Data.Exception = exception;
            Data.Failed = true;
            return this;
        }

        public IState Fail(IFilteringError filteringError)
        {
            Data.FilteringErrors.Add(filteringError);
            Data.Invalid = true;
            return this;
        }

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

        public IState<TR> Fail<TR>()
        {
            Data.Failed = true;
            return new State<TR>(default, Data);
        }

        public IState<TR> Fail<TR>(Exception exception)
        {
            Data.Exception = exception;
            Data.Failed = true;
            return new State<TR>(default, Data);
        }

        public new IState<T> Fail(IFilteringError filteringError)
        {
            Data.FilteringErrors.Add(filteringError);
            Data.Invalid = true;
            return this;
        }

        public IState Next() => new State(Data);
    }
}
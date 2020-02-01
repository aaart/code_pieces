using System;
using System.Collections.Generic;

namespace Flow
{
    public class State<TFilteringError> : IState<TFilteringError>
    {
        protected internal State(StateData<TFilteringError> stateData)
        {
            Data = stateData;
        }
        protected StateData<TFilteringError> Data { get; }

        IEventReceiver IState<TFilteringError>.EventReceiver => Data.EventReceiver;
        public IEnumerable<TFilteringError> FilteringErrors => Data.FilteringErrors;
        public Exception Exception => Data.Exception;
        public bool Broken => Data.Broken;
        public bool Invalid => Data.Invalid;

        public IState<TFilteringError> Fail()
        {
            Data.Broken = true;
            return this;
        }

        public IState<TFilteringError> Fail(Exception exception)
        {
            Data.Exception = exception;
            Data.Broken = true;
            return this;
        }

        public IState<TFilteringError> Invalidate(TFilteringError filteringError)
        {
            Data.FilteringErrors.Add(filteringError);
            Data.Invalid = true;
            return this;
        }

        public void Done() => Data.Dispose();
    }

    public class State<T, TFilteringError> : State<TFilteringError>, IState<T, TFilteringError>
    {
        public State(T result, StateData<TFilteringError> stateData)
            : base(stateData)
        {
            Result = result;
        }

        public T Result { get; }
        
        public IState<TR, TFilteringError> Next<TR>(TR result) => new State<TR, TFilteringError>(result, Data);

        public IState<TR, TFilteringError> Fail<TR>()
        {
            Data.Broken = true;
            return new State<TR, TFilteringError>(default, Data);
        }

        public IState<TR, TFilteringError> Fail<TR>(Exception exception)
        {
            Data.Exception = exception;
            Data.Broken = true;
            return new State<TR, TFilteringError>(default, Data);
        }

        public new IState<T, TFilteringError> Invalidate(TFilteringError filteringError)
        {
            Data.FilteringErrors.Add(filteringError);
            Data.Invalid = true;
            return this;
        }

        public IState<TFilteringError> Next() => new State<TFilteringError>(Data);
    }
}
using System;
using System.Collections.Generic;

namespace Flow
{
    public class StepResult<TEventReceiver> : IState
        where TEventReceiver : IEventReceiver
    {
        protected internal StepResult(StepState<TEventReceiver> stepState)
        {
            Data = stepState;
        }
        protected StepState<TEventReceiver> Data { get; }

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

    public class StepResult<T, TEventReceiver> : StepResult<TEventReceiver>, IState<T>
        where TEventReceiver : IEventReceiver
    {
        public StepResult(T result, StepState<TEventReceiver> stepState)
            : base(stepState)
        {
            Result = result;
        }

        public T Result { get; }

        public IState<TR> Next<TR>(TR result) => new StepResult<TR, TEventReceiver>(result, Data);

        public IState<TR> Skip<TR>() => new StepResult<TR, TEventReceiver>(default, Data);

        public IState<TR> Fail<TR>()
        {
            Data.Failed = true;
            return new StepResult<TR, TEventReceiver>(default, Data);
        }

        public IState Next() => new StepResult<TEventReceiver>(Data);
    }
}
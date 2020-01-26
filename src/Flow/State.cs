using System;
using System.Collections.Generic;

namespace Flow
{
    public class State<TEventReceiver> : IState
        where TEventReceiver : IEventReceiver
    {
        protected static State<TEventReceiver> ConstructorProxy(IEnumerable<IFilteringError> errors,
            TEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, TEventReceiver> onDisposing, Exception exception, bool failed) =>
            new State<TEventReceiver>(eventReceiver, onDisposing, errors, exception, failed);

        private readonly List<IFilteringError> _errors = new List<IFilteringError>();
        protected Action<IEnumerable<IFilteringError>, TEventReceiver> OnStateDone { get; }
        
        protected State(
            TEventReceiver eventReceiver, 
            Action<IEnumerable<IFilteringError>, TEventReceiver> onStateDone,
            IEnumerable<IFilteringError> errors, 
            Exception exception, 
            bool failed)
        {
            EventReceiver = eventReceiver;
            _errors.AddRange(errors);
            OnStateDone = onStateDone;
            Exception = exception;
            Failed = failed;
        }

        IEventReceiver IState.EventReceiver => EventReceiver; 
        public TEventReceiver EventReceiver { get; }
        public IEnumerable<IFilteringError> Errors => _errors;
        public Exception Exception { get; set; }
        public bool Failed { get; private set; }

        public IState Skip() => this;
        public IState Fail()
        {
            Failed = true;
            return this;
        }

        public void PublishError(IFilteringError filteringError) => _errors.Add(filteringError);

        public void Done() => OnStateDone(Errors, EventReceiver);
    }

    public class State<T, TEventReceiver> : State<TEventReceiver>, IState<T>
        where TEventReceiver : IEventReceiver
    {
        protected static State<TR, TEventReceiver> ConstructorProxy<TR>(TR result, IEnumerable<IFilteringError> errors,
            TEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, TEventReceiver> onDisposing, Exception exception, bool failed) =>
            new State<TR, TEventReceiver>(result, eventReceiver, onDisposing, errors, exception, failed);

        public State(T result, TEventReceiver eventReceiver)
            : this(result, eventReceiver, (e, er) => { }, new List<IFilteringError>(), null, false)
        {
        }
        
        public State(T result, TEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, TEventReceiver> onStateDone, bool failed = false)
            : this(result, eventReceiver, onStateDone, new List<IFilteringError>(), null, failed)
        {
        }

        protected State(T result, TEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, TEventReceiver> onStateDone, IEnumerable<IFilteringError> errors, bool failed)
            : this(result, eventReceiver, onStateDone, errors, null, failed)
        {
            Result = result;
        }

        private State(
            T result, 
            TEventReceiver eventReceiver, 
            Action<IEnumerable<IFilteringError>, TEventReceiver> onStateDone, 
            IEnumerable<IFilteringError> errors,
            Exception exception, 
            bool failed)
            : base(eventReceiver, onStateDone, errors, exception, failed)
        {
            Result = result;
        }

        public T Result { get; }

        public IState<TR> Next<TR>(TR result) => ConstructorProxy(result, Errors, EventReceiver, OnStateDone, Exception, Failed);

        public IState<TR> Skip<TR>() => ConstructorProxy<TR>(default, Errors, EventReceiver, OnStateDone, Exception, Failed);
        public IState<TR> Fail<TR>() => ConstructorProxy<TR>(default, Errors, EventReceiver, OnStateDone, Exception, true);

        public IState Next() => ConstructorProxy(Errors, EventReceiver, OnStateDone, Exception, Failed);
    }
}
using System;
using System.Collections.Generic;

namespace Flow
{
    public class State<TEventReceiver> : IState
        where TEventReceiver : IEventReceiver
    {
        protected static State<TEventReceiver> ConstructorProxy(IEnumerable<IFilteringError> errors,
            TEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, TEventReceiver> onDisposing, Exception exception) =>
            new State<TEventReceiver>(errors, eventReceiver, onDisposing, exception);

        private readonly List<IFilteringError> _errors = new List<IFilteringError>();
        protected Action<IEnumerable<IFilteringError>, TEventReceiver> OnStateDone { get; }
        
        protected State(IEnumerable<IFilteringError> errors, TEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, TEventReceiver> onStateDone, Exception exception)
        {
            EventReceiver = eventReceiver;
            _errors.AddRange(errors);
            OnStateDone = onStateDone;
            Exception = exception;
        }

        IEventReceiver IState.EventReceiver => EventReceiver; 
        public TEventReceiver EventReceiver { get; }
        public IEnumerable<IFilteringError> Errors => _errors;
        public Exception Exception { get; set; }

        public IState Skip() => this;

        public void PublishError(IFilteringError filteringError) => _errors.Add(filteringError);

        public void Done() => OnStateDone(Errors, EventReceiver);
    }

    public class State<T, TEventReceiver> : State<TEventReceiver>, IState<T>
        where TEventReceiver : IEventReceiver
    {
        protected static State<TR, TEventReceiver> ConstructorProxy<TR>(TR result, IEnumerable<IFilteringError> errors,
            TEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, TEventReceiver> onDisposing, Exception exception) =>
            new State<TR, TEventReceiver>(result, errors, eventReceiver, onDisposing, exception);

        public State(T result, TEventReceiver eventReceiver)
            : this(result, new List<IFilteringError>(), eventReceiver, (e, er) => { }, null)
        {
        }
        
        public State(T result, TEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, TEventReceiver> onDisposing)
            : this(result, new List<IFilteringError>(), eventReceiver, onDisposing, null)
        {
        }

        protected State(T result, IEnumerable<IFilteringError> errors, TEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, TEventReceiver> onStateDone)
            : this(result, errors, eventReceiver, onStateDone, null)
        {
            Result = result;
        }

        private State(T result, IEnumerable<IFilteringError> errors, TEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, TEventReceiver> onStateDone, Exception exception)
            : base(errors, eventReceiver, onStateDone, exception)
        {
            Result = result;
        }

        public T Result { get; }

        public IState<TR> Next<TR>(TR result) => ConstructorProxy(result, Errors, EventReceiver, OnStateDone, Exception);

        public IState<TR> Skip<TR>() => ConstructorProxy<TR>(default, Errors, EventReceiver, OnStateDone, Exception);

        public IState Next() => ConstructorProxy(Errors, EventReceiver, OnStateDone, Exception);
    }
}
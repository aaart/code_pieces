using System;
using System.Collections.Generic;

namespace Flow
{
    public class State<TEventReceiver> : IState
        where TEventReceiver : IEventReceiver
    {
        protected static State<TEventReceiver> ConstructorProxy(IEnumerable<IFilteringError> errors, TEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, TEventReceiver> onDisposing) =>
            new State<TEventReceiver>(errors, eventReceiver, onDisposing);

        protected TEventReceiver EventReceiver { get; }
        private readonly List<IFilteringError> _errors = new List<IFilteringError>();
        protected Action<IEnumerable<IFilteringError>, TEventReceiver> OnDisposing { get; }
        
        protected State(IEnumerable<IFilteringError> errors, TEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, TEventReceiver> onDisposing)
        {
            EventReceiver = eventReceiver;
            _errors.AddRange(errors);
            OnDisposing = onDisposing;
        }

        public IEnumerable<IFilteringError> Errors => _errors;
        public Exception Exception { get; set; }

        public IState Skip() => this;

        public void PublishError(IFilteringError filteringError) => _errors.Add(filteringError);

        public void Receive(IEvent @event) => EventReceiver.Receive(@event);

        public void Dispose() => OnDisposing(Errors, EventReceiver);
    }

    public class State<T, TEventReceiver> : State<TEventReceiver>, IState<T>
        where TEventReceiver : IEventReceiver
    {
        protected static State<TR, TEventReceiver> ConstructorProxy<TR>(TR result, IEnumerable<IFilteringError> errors, TEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, TEventReceiver> onDisposing) =>
            new State<TR, TEventReceiver>(result, errors, eventReceiver, onDisposing);

        public State(T result, TEventReceiver eventReceiver)
            : this(result, new List<IFilteringError>(), eventReceiver, (e, er) => { })
        {
        }
        
        public State(T result, TEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, TEventReceiver> onDisposing)
            : this(result, new List<IFilteringError>(), eventReceiver, onDisposing)
        {
        }

        protected State(T result, IEnumerable<IFilteringError> errors, TEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, TEventReceiver> onDisposing)
            : base(errors, eventReceiver, onDisposing)
        {
            Result = result;
        }

        public T Result { get; }

        public IState<TR> Next<TR>(TR result) => ConstructorProxy(result, Errors, EventReceiver, OnDisposing);

        public IState<TR> Skip<TR>() => ConstructorProxy<TR>(default, Errors, EventReceiver, OnDisposing);

        public IState Next() => ConstructorProxy(Errors, EventReceiver, OnDisposing);
    }
}
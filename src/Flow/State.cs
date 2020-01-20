using System;
using System.Collections.Generic;

namespace Flow
{
    public class State<TEventReceiver> : IState
        where TEventReceiver : IEventReceiver
    {
        protected static State<TEventReceiver> ConstructorProxy(IEnumerable<IError> errors, TEventReceiver eventReceiver, Action<IEnumerable<IError>, TEventReceiver> onDisposing) =>
            new State<TEventReceiver>(errors, eventReceiver, onDisposing);

        protected TEventReceiver EventReceiver { get; }
        private readonly List<IError> _errors = new List<IError>();
        protected Action<IEnumerable<IError>, TEventReceiver> OnDisposing { get; }
        
        protected State(IEnumerable<IError> errors, TEventReceiver eventReceiver, Action<IEnumerable<IError>, TEventReceiver> onDisposing)
        {
            EventReceiver = eventReceiver;
            _errors.AddRange(errors);
            OnDisposing = onDisposing;
        }

        public IEnumerable<IError> Errors => _errors;

        public IState Skip() => this;

        public void PublishError(IError error) => _errors.Add(error);

        public void Receive(IEvent @event) => throw new System.NotImplementedException();

        public void Dispose() => OnDisposing(Errors, EventReceiver);
    }

    public class State<T, TEventReceiver> : State<TEventReceiver>, IState<T>
        where TEventReceiver : IEventReceiver
    {
        protected static State<TR, TEventReceiver> ConstructorProxy<TR>(TR result, IEnumerable<IError> errors, TEventReceiver eventReceiver, Action<IEnumerable<IError>, TEventReceiver> onDisposing) =>
            new State<TR, TEventReceiver>(result, errors, eventReceiver, onDisposing);

        public State(T result, TEventReceiver eventReceiver)
            : this(result, new List<IError>(), eventReceiver, (e, er) => { })
        {
        }

        protected State(T result, IEnumerable<IError> errors, TEventReceiver eventReceiver, Action<IEnumerable<IError>, TEventReceiver> onDisposing)
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
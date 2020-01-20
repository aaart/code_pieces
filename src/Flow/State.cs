using System;
using System.Collections.Generic;

namespace Flow
{
    public class State : IState
    {
        protected static State ConstructorProxy(IEnumerable<IError> errors, IEventReceiver eventReceiver, Action<IEnumerable<IError>, IEventReceiver> onDisposing) => 
            new State(errors, eventReceiver, onDisposing);

        protected IEventReceiver EventReceiver { get; }
        private readonly List<IError> _errors = new List<IError>();
        protected Action<IEnumerable<IError>, IEventReceiver> OnDisposing { get; }

        public State()
            : this(new List<IError>(), new StandardEventReceiver(), (e, er) => { })
        {
        }

        protected State(IEnumerable<IError> errors, IEventReceiver eventReceiver, Action<IEnumerable<IError>, IEventReceiver> onDisposing)
        {
            EventReceiver = eventReceiver;
            _errors.AddRange(errors);
            OnDisposing = onDisposing;
        }

        public IEnumerable<IError> Errors => _errors;

        public IState Skip() => this;

        public void PublishError(IError error) => _errors.Add(error);

        public void Receive(IEvent @event)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            OnDisposing(Errors, EventReceiver);
        }
    }

    public class State<T> : State, IState<T>
    {
        public State(T result)
            : this(result, new List<IError>(), new StandardEventReceiver(), (e, er) => { })
        {
        }

        protected State(T result, IEnumerable<IError> errors, IEventReceiver eventReceiver, Action<IEnumerable<IError>, IEventReceiver> onDisposing)
            : base(errors, eventReceiver, onDisposing)
        {
            Result = result;
        }

        public T Result { get; }

        public IState<TR> Next<TR>(TR result)
        {
            return new State<TR>(result, Errors, EventReceiver, OnDisposing);
        }

        public IState<TR> Skip<TR>()
        {
            return new State<TR>(default, Errors, EventReceiver, OnDisposing);
        }

        public IState Next()
        {
            return ConstructorProxy(Errors, EventReceiver, OnDisposing);
        }
    }
}
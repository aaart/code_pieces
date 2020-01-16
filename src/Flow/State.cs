using System.Collections.Generic;
using System.Linq;

namespace Flow
{
    public class State : IState
    {
        private readonly List<IError> _errors = new List<IError>();


        public State()
            : this(new List<IError>(), new StandardEventReceiver())
        {
        }

        public State(IEnumerable<IError> errors, IEventReceiver eventReceiver)
        {
            _errors.AddRange(errors);
            EventReceiver = eventReceiver;
        }

        public IState Skip() => this;

        public void PublishError(IError error) => _errors.Add(error);

        public IEnumerable<IError> Errors => _errors;
        public IEventReceiver EventReceiver { get; }
    }

    public class State<T> : State, IState<T>
    {
        public State(T result)
            : this(result, new List<IError>(), new StandardEventReceiver())
        {
        }

        public State(T result, IEnumerable<IError> errors, IEventReceiver eventReceiver)
            : base(errors, eventReceiver)
        {
            Result = result;
        }

        public T Result { get; }
        
        public IState<TR> Clone<TR>(TR result)
        {
            return new State<TR>(result, Errors, EventReceiver);
        }

        public IState<TR> Skip<TR>()
        {
            return new State<TR>(default, Errors, EventReceiver);
        }

        public IState ToVoid()
        {
            return new State(Errors, EventReceiver);
        }
    }
}
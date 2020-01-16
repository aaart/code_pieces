using System.Collections;
using System.Collections.Generic;

namespace Flow
{
    public class StateStack : IState
    {
        private readonly Stack _results = new Stack();
        private readonly Stack<IError> _errors = new Stack<IError>();
        private readonly StandardEventReceiver _standardEventReceiver = new StandardEventReceiver();

        public StateStack(object target)
        {
            PushResult(target);
        }

        public T CurrentResult<T>() => (T)_results.Peek();
        public void PushResult(object o) => _results.Push(o);

        public IError CurrentError => _errors.Peek();

        public void PushError(IError error) => _errors.Push(error);

        public IEnumerable<IError> Errors => _errors;
        public IEventReceiver EventReceiver => _standardEventReceiver;
    }
}
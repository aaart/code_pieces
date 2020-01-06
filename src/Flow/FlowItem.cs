using System;
using System.Collections;
using System.Collections.Generic;

namespace Flow
{
    public class FlowItem<T> : IBeginFlow<T>, IValidatedVerified<T>
    {
        private readonly List<Action<Stack>> _actions;
        private readonly Stack _resultStack;

        public FlowItem(T input)
            : this(input, new List<Action<Stack>>(), new Stack())
        {
        }

        private FlowItem(List<Action<Stack>> actions, Stack resultStack)
        {
            _actions = actions;
            _resultStack = resultStack;
        }
        private FlowItem(T input, List<Action<Stack>> actions, Stack resultStack)
            : this(actions, resultStack)
        {
            _resultStack.Push(input);
        }

        public IBeginFlow<T> Validate<TR>(Func<T, TR> validationTarget, Func<TR, bool> validator, Func<IError> error)
        {
            throw new NotImplementedException();
        }

        public IBeginFlow<T> Validate(Func<T, bool> validator, Func<IError> error)
        {
            throw new NotImplementedException();
        }

        public IBeginFlow<T> Validate<TR>(Func<T, TR> validationTarget, IFilter<TR> filter)
        {
            throw new NotImplementedException();
        }

        public IBeginFlow<T> Validate(IFilter<T> filter)
        {
            throw new NotImplementedException();
        }

        public IValidatedVerified<TR> Apply<TR>(Func<T, TR> apply)
        {
            _actions.Add(stack => stack.Push(apply((T)stack.Peek())));
            return new FlowItem<TR>(_actions, _resultStack);
        }

        public IValidated<T> Verify<TR>(Func<T, TR> verificationTarget, Func<TR, bool> check, Func<IError> error)
        {
            throw new NotImplementedException();
        }

        public IValidated<T> Verify(Func<T, bool> check, Func<IError> error)
        {
            throw new NotImplementedException();
        }

        public IValidated<T> Verify(IFilter<T> filter)
        {
            throw new NotImplementedException();
        }

        public IValidated<T> Verify<TR>(Func<T, TR> verificationTarget, IFilter<TR> filter)
        {
            throw new NotImplementedException();
        }

        public IPipeline Finalize(Action<T> execution)
        {
            _actions.Add(stack => execution((T)stack.Peek()));
            return new Pipeline(_actions, _resultStack);
        }

        public IPipeline<TR> Finalize<TR>(Func<T, TR> execution)
        {
            _actions.Add(stack => _resultStack.Push(execution((T)stack.Peek())));
            return new Pipeline<TR>(_actions, _resultStack);
        }
    }
}
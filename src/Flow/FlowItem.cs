using System;
using System.Linq;

namespace Flow
{
    public class FlowItem<T> : IBeginFlow<T>, IValidatedVerified<T>
    {
        private readonly Action<IFlowItemToken> _action;

        public FlowItem(T input)
            : this(x => x.PushResult(input))
        {
        }

        private FlowItem(Action<IFlowItemToken> action)
        {
            _action = action;
        }

        public IBeginFlow<T> Validate<TR>(Func<T, TR> validationTarget, Func<TR, bool> validator, Func<IError> error)
        {
            throw new NotImplementedException();
        }

        public IBeginFlow<T> Validate(Func<T, bool> validator, Func<IError> error)
        {
            void NextAction(IFlowItemToken x)
            {
                _action(x);
                T target = (T)x.CurrentResult;
                if (!validator(target))
                {
                    x.PushError(error());
                }
            }

            return new FlowItem<T>(NextAction);
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
            void NextAction(IFlowItemToken x)
            {
                _action(x);
                T target = (T)x.CurrentResult;
                x.PushResult(apply(target));
            }

            return new FlowItem<TR>(NextAction);
        }

        public IValidated<T> Verify<TR>(Func<T, TR> verificationTarget, Func<TR, bool> check, Func<IError> error)
        {
            throw new NotImplementedException();
        }

        public IValidated<T> Verify(Func<T, bool> check, Func<IError> error)
        {
            void NextAction(IFlowItemToken x)
            {
                _action(x);
                T target = (T)x.CurrentResult;
                if (!check(target))
                {
                    x.PushError(error());
                }
            }

            return new FlowItem<T>(NextAction);
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
            void NextAction(IFlowItemToken x)
            {
                _action(x);
                if (!x.Errors.Any())
                {
                    T target = (T)x.CurrentResult;
                    execution(target);
                }
            }

            return new Pipeline(NextAction);
        }

        public IPipeline<TR> Finalize<TR>(Func<T, TR> execution)
        {
            void NextAction(IFlowItemToken x)
            {
                _action(x);
                if (!x.Errors.Any())
                {
                    T target = (T)x.CurrentResult;
                    x.PushResult(execution(target));
                }
            }

            return new Pipeline<TR>(NextAction);
        }
    }
}
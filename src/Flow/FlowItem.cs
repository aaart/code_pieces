using System;
using System.Linq;

namespace Flow
{
    public class FlowItem<T> : IBeginFlow<T>, IValidatedVerified<T>
    {
        private readonly Action<IFlowItemToken> _action;

        public FlowItem(T input)
            : this(token => token.PushResult(input))
        {
        }

        private FlowItem(Action<IFlowItemToken> action)
        {
            _action = action;
        }

        public IBeginFlow<T> Validate<TR>(Func<T, TR> transform, Func<TR, bool> validator, Func<IError> error)
        {
            void NextAction(IFlowItemToken token)
            {
                DecorateValidateVerify(token, transform, new LambdaFilter<TR>(validator, error));
            }

            return new FlowItem<T>(NextAction);
        }

        public IBeginFlow<T> Validate(Func<T, bool> validator, Func<IError> error)
        {
            void NextAction(IFlowItemToken token)
            {
                DecorateValidateVerify(token, x => x, new LambdaFilter<T>(validator, error));
            }

            return new FlowItem<T>(NextAction);
        }

        public IBeginFlow<T> Validate<TR>(Func<T, TR> transform, IFilter<TR> filter)
        {
            void NextAction(IFlowItemToken token)
            {
                DecorateValidateVerify(token, transform, filter);
            }

            return new FlowItem<T>(NextAction);
        }

        public IBeginFlow<T> Validate(IFilter<T> filter)
        {
            void NextAction(IFlowItemToken token)
            {
                DecorateValidateVerify(token, x => x, filter);
            }

            return new FlowItem<T>(NextAction);
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

        public IValidated<T> Verify<TR>(Func<T, TR> transform, Func<TR, bool> check, Func<IError> error)
        {
            void NextAction(IFlowItemToken token)
            {
                DecorateValidateVerify(token, transform, new LambdaFilter<TR>(check, error));
            }

            return new FlowItem<T>(NextAction);
        }

        public IValidated<T> Verify(Func<T, bool> check, Func<IError> error)
        {
            void NextAction(IFlowItemToken token)
            {
                DecorateValidateVerify(token, x => x, new LambdaFilter<T>(check, error));
            }

            return new FlowItem<T>(NextAction);
        }

        public IValidated<T> Verify(IFilter<T> filter)
        {
            void NextAction(IFlowItemToken token)
            {
                DecorateValidateVerify(token, x => x, filter);
            }

            return new FlowItem<T>(NextAction);
        }

        public IValidated<T> Verify<TR>(Func<T, TR> transform, IFilter<TR> filter)
        {
            void NextAction(IFlowItemToken token)
            {
                DecorateValidateVerify(token, transform, filter);
            }

            return new FlowItem<T>(NextAction);
        }

        public IPipeline Finalize(Action<T> execution)
        {
            void NextAction(IFlowItemToken token)
            {
                DecorateExecution(token, argument => execution(argument));
            }

            return new Pipeline(NextAction);
        }

        public IPipeline<TR> Finalize<TR>(Func<T, TR> execution)
        {
            void NextAction(IFlowItemToken token)
            {
                DecorateExecution(token, argument => token.PushResult(execution(argument)));
            }

            return new Pipeline<TR>(NextAction);
        }

        public void DecorateExecution(IFlowItemToken token, Action<T> target)
        {
            _action(token);
            if (!token.Errors.Any())
            {
                target((T)token.CurrentResult);
            }
        }

        public void DecorateValidateVerify<TK>(IFlowItemToken token, Func<T, TK> transform, IFilter<TK> filter)
        {
            _action(token);
            TK target = transform((T)token.CurrentResult);
            if (!filter.Check(target, out IError error))
            {
                token.PushError(error);
            }
        }
    }
}
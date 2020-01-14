using System;
using System.Linq;

namespace Flow
{
    public class FlowItem<T> : IBeginFlow<T>, IValidatedVerified<T>
    {
        private readonly Action<IFlowItemState> _action;

        public FlowItem(T input)
            : this(state => state.PushResult(input))
        {
        }

        private FlowItem(Action<IFlowItemState> action)
        {
            _action = action;
        }

        public IBeginFlow<T> Validate<TR>(Func<T, TR> transform, Func<TR, bool> validator, Func<IError> error)
        {
            void NextAction(IFlowItemState state)
            {
                DecorateValidateVerify(state, transform, new LambdaFilter<TR>(validator, error));
            }

            return Clone<T>(NextAction);
        }

        public IBeginFlow<T> Validate(Func<T, bool> validator, Func<IError> error)
        {
            void NextAction(IFlowItemState state)
            {
                DecorateValidateVerify(state, x => x, new LambdaFilter<T>(validator, error));
            }

            return Clone<T>(NextAction);
        }

        public IBeginFlow<T> Validate<TR>(Func<T, TR> transform, IFilter<TR> filter)
        {
            void NextAction(IFlowItemState state)
            {
                DecorateValidateVerify(state, transform, filter);
            }

            return Clone<T>(NextAction);
        }

        public IBeginFlow<T> Validate(IFilter<T> filter)
        {
            void NextAction(IFlowItemState state)
            {
                DecorateValidateVerify(state, x => x, filter);
            }

            return Clone<T>(NextAction);
        }

        public IValidatedVerified<TR> Apply<TR>(Func<T, TR> apply)
        {
            void NextAction(IFlowItemState state)
            {
                _action(state);
                T target = (T)state.CurrentResult;
                state.PushResult(apply(target));
            }

            return Clone<TR>(NextAction);
        }

        public IValidated<T> Verify<TR>(Func<T, TR> transform, Func<TR, bool> check, Func<IError> error)
        {
            void NextAction(IFlowItemState state)
            {
                DecorateValidateVerify(state, transform, new LambdaFilter<TR>(check, error));
            }

            return Clone<T>(NextAction);
        }

        public IValidated<T> Verify(Func<T, bool> check, Func<IError> error)
        {
            void NextAction(IFlowItemState state)
            {
                DecorateValidateVerify(state, x => x, new LambdaFilter<T>(check, error));
            }

            return Clone<T>(NextAction);
        }

        public IValidated<T> Verify(IFilter<T> filter)
        {
            void NextAction(IFlowItemState state)
            {
                DecorateValidateVerify(state, x => x, filter);
            }

            return Clone<T>(NextAction);
        }

        public IValidated<T> Verify<TR>(Func<T, TR> transform, IFilter<TR> filter)
        {
            void NextAction(IFlowItemState state)
            {
                DecorateValidateVerify(state, transform, filter);
            }

            return Clone<T>(NextAction);
        }

        public IValidatedVerified<T> Publish<TE>(Func<T, TE> publishEvent)
            where TE : IEvent
        {
            void NextAction(IFlowItemState state)
            {
                _action(state);
                T target = (T)state.CurrentResult;
                state.EventReceiver.Receive(publishEvent(target));
            }

            return Clone<T>(NextAction);
        }

        public IPipeline Finalize(Action<T> execution)
        {
            void NextAction(IFlowItemState state)
            {
                DecorateExecution(state, argument => execution(argument));
            }

            return new Pipeline(NextAction);
        }

        public IPipeline<TR> Finalize<TR>(Func<T, TR> execution)
        {
            void NextAction(IFlowItemState state)
            {
                DecorateExecution(state, argument => state.PushResult(execution(argument)));
            }

            return new Pipeline<TR>(NextAction);
        }

        public void DecorateExecution(IFlowItemState state, Action<T> target)
        {
            _action(state);
            if (!state.Errors.Any())
            {
                target((T)state.CurrentResult);
            }
        }

        public void DecorateValidateVerify<TK>(IFlowItemState state, Func<T, TK> transform, IFilter<TK> filter)
        {
            _action(state);
            TK target = transform((T)state.CurrentResult);
            if (!filter.Check(target, out IError error))
            {
                state.PushError(error);
            }
        }

        private FlowItem<TR> Clone<TR>(Action<IFlowItemState> action)
        {
            return new FlowItem<TR>(action);
        }
    }
}
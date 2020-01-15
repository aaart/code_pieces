using System;
using System.Linq;

namespace Flow
{
    public class FlowItem<T> : IBeginFlow<T>, IValidatedVerified<T>
    {
        private readonly Func<IFlowItemState> _method;

        public FlowItem(T input, IFlowItemState state)
            : this(() =>
            {
                state.PushResult(input);
                return state;
            })
        {
        }

        private FlowItem(Func<IFlowItemState> method)
        {
            _method = method;
        }

        public IBeginFlow<T> Validate<TR>(Func<T, TR> transform, Func<TR, bool> validator, Func<IError> error) => 
            Clone<T>(() => Decorate(transform, new LambdaFilter<TR>(validator, error)));

        public IBeginFlow<T> Validate(Func<T, bool> validator, Func<IError> error) => 
            Clone<T>(() => Decorate(x => x, new LambdaFilter<T>(validator, error)));

        public IBeginFlow<T> Validate<TR>(Func<T, TR> transform, IFilter<TR> filter) => 
            Clone<T>(() => Decorate(transform, filter));

        public IBeginFlow<T> Validate(IFilter<T> filter) => 
            Clone<T>(() => Decorate(x => x, filter));

        public IValidatedVerified<TR> Apply<TR>(Func<T, TR> apply) => 
            Clone<TR>(() => Decorate((argument, state) => state.PushResult(apply((T)state.CurrentResult))));

        public IValidated<T> Verify<TR>(Func<T, TR> transform, Func<TR, bool> check, Func<IError> error) => 
            Clone<T>(() => Decorate(transform, new LambdaFilter<TR>(check, error)));

        public IValidated<T> Verify(Func<T, bool> check, Func<IError> error) => 
            Clone<T>(() => Decorate(x => x, new LambdaFilter<T>(check, error)));

        public IValidated<T> Verify(IFilter<T> filter) => 
            Clone<T>(() => Decorate(x => x, filter));

        public IValidated<T> Verify<TR>(Func<T, TR> transform, IFilter<TR> filter) => 
            Clone<T>(() => Decorate(transform, filter));

        public IValidatedVerified<T> Publish<TE>(Func<T, TE> publishEvent) where TE : IEvent =>
            Clone<T>(() => Decorate((argument, state) => state.EventReceiver.Receive(publishEvent((T)state.CurrentResult))));

        public IPipeline Finalize(Action<T> execution) => 
            new Pipeline(() => Decorate((argument, state) => execution(argument)));

        public IPipeline<TR> Finalize<TR>(Func<T, TR> execution) => 
            new Pipeline<TR>(() => Decorate((argument, state) => state.PushResult(execution(argument))));

        public IFlowItemState Decorate(Action<T, IFlowItemState> target)
        {
            var state = _method();
            if (!state.Errors.Any())
            {
                target((T)state.CurrentResult, state);
            }

            return state;
        }

        public IFlowItemState Decorate<TK>(Func<T, TK> transform, IFilter<TK> filter)
        {
            var state = _method();
            TK target = transform((T)state.CurrentResult);
            if (!filter.Check(target, out IError error))
            {
                state.PushError(error);
            }

            return state;
        }

        private FlowItem<TR> Clone<TR>(Func<IFlowItemState> method) => new FlowItem<TR>(method);
    }
}
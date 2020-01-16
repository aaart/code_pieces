using System;
using System.Linq;

namespace Flow
{
    public class FlowItem<T> : IBeginFlow<T>, IValidatedVerified<T>
    {
        private readonly Func<IState<T>> _method;
        
        internal FlowItem(Func<IState<T>> method)
        {
            _method = method;
        }

        public IBeginFlow<T> Validate<TR>(Func<T, TR> transform, Func<TR, bool> validator, Func<IError> error) => 
            Clone(() => Decorate(transform, new LambdaFilter<TR>(validator, error)));

        public IBeginFlow<T> Validate(Func<T, bool> validator, Func<IError> error) => 
            Clone(() => Decorate(x => x, new LambdaFilter<T>(validator, error)));

        public IBeginFlow<T> Validate<TR>(Func<T, TR> transform, IFilter<TR> filter) => 
            Clone(() => Decorate(transform, filter));

        public IBeginFlow<T> Validate(IFilter<T> filter) => 
            Clone(() => Decorate(x => x, filter));

        public IValidatedVerified<TR> Apply<TR>(Func<T, TR> apply) => 
            Clone(() => Decorate((argument, state) => state.Clone(apply(state.Result))));

        public IValidated<T> Verify<TR>(Func<T, TR> transform, Func<TR, bool> check, Func<IError> error) => 
            Clone(() => Decorate(transform, new LambdaFilter<TR>(check, error)));

        public IValidated<T> Verify(Func<T, bool> check, Func<IError> error) => 
            Clone(() => Decorate(x => x, new LambdaFilter<T>(check, error)));

        public IValidated<T> Verify(IFilter<T> filter) => 
            Clone(() => Decorate(x => x, filter));

        public IValidated<T> Verify<TR>(Func<T, TR> transform, IFilter<TR> filter) => 
            Clone(() => Decorate(transform, filter));

        public IValidatedVerified<T> Publish<TE>(Func<T, TE> publishEvent) where TE : IEvent =>
            Clone(() => Decorate((argument, state) =>
            {
                state.EventReceiver.Receive(publishEvent(state.Result));
                return state;
            }));

        public IPipeline Finalize(Action<T> execution) => 
            new Pipeline(() => Decorate((argument, state) =>
            {
                execution(argument);
                return state.ToVoid();
            }));

        public IPipeline<TR> Finalize<TR>(Func<T, TR> execution) => 
            new Pipeline<TR>(() => Decorate((argument, state) => state.Clone(execution(argument))));

        private IState Decorate(Func<T, IState<T>, IState> target)
        {
            var state = _method();
            if (!state.Errors.Any())
            {
                return target(state.Result, state);
            }

            return state.Skip();
        }
        
        private IState<TR> Decorate<TR>(Func<T, IState<T>, IState<TR>> target)
        {
            var state = _method();
            if (!state.Errors.Any())
            {
                return target(state.Result, state);
            }
            return state.Skip<TR>();
        }

        private IState<T> Decorate<TK>(Func<T, TK> transform, IFilter<TK> filter)
        {
            var state = _method();
            TK target = transform(state.Result);
            if (!filter.Check(target, out IError error))
            {
                state.PublishError(error);
            }

            return state;
        }

        private FlowItem<TR> Clone<TR>(Func<IState<TR>> method) => new FlowItem<TR>(method);
    }
}
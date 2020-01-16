using System;
using System.Linq;

namespace Flow
{
    public class FlowItem<T> : IBeginFlow<T>, IValidatedVerified<T>
    {
        private readonly Func<IState> _method;
        
        internal FlowItem(Func<IState> method)
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
            Clone<TR>(() => Decorate((argument, state) => state.PushResult(apply(state.CurrentResult<T>()))));

        public IValidated<T> Verify<TR>(Func<T, TR> transform, Func<TR, bool> check, Func<IError> error) => 
            Clone<T>(() => Decorate(transform, new LambdaFilter<TR>(check, error)));

        public IValidated<T> Verify(Func<T, bool> check, Func<IError> error) => 
            Clone<T>(() => Decorate(x => x, new LambdaFilter<T>(check, error)));

        public IValidated<T> Verify(IFilter<T> filter) => 
            Clone<T>(() => Decorate(x => x, filter));

        public IValidated<T> Verify<TR>(Func<T, TR> transform, IFilter<TR> filter) => 
            Clone<T>(() => Decorate(transform, filter));

        public IValidatedVerified<T> Publish<TE>(Func<T, TE> publishEvent) where TE : IEvent =>
            Clone<T>(() => Decorate((argument, state) => state.EventReceiver.Receive(publishEvent(state.CurrentResult<T>()))));

        public IPipeline Finalize(Action<T> execution) => 
            new Pipeline(() => Decorate((argument, state) => execution(argument)));

        public IPipeline<TR> Finalize<TR>(Func<T, TR> execution) => 
            new Pipeline<TR>(() => Decorate((argument, state) => state.PushResult(execution(argument))));

        public IState Decorate(Action<T, IState> target)
        {
            var state = _method();
            if (!state.Errors.Any())
            {
                target(state.CurrentResult<T>(), state);
            }

            return state;
        }

        public IState Decorate<TK>(Func<T, TK> transform, IFilter<TK> filter)
        {
            var state = _method();
            TK target = transform(state.CurrentResult<T>());
            if (!filter.Check(target, out IError error))
            {
                state.PushError(error);
            }

            return state;
        }

        private FlowItem<TR> Clone<TR>(Func<IState> method) => new FlowItem<TR>(method);
    }
}
using System;

namespace Flow
{
    public class Step<T, TFilteringError> : IFlow<T, TFilteringError>, IValidatedVerified<T, TFilteringError>, IEventSource<T, TFilteringError>
    {
        private readonly Func<IState<T, TFilteringError>> _method;
        
        public Step(Func<IState<T, TFilteringError>> method)
        {
            _method = method;
        }

        public IFlow<T, TFilteringError> Validate<TR>(Func<T, TR> transform, Func<TR, bool> validator, Func<TFilteringError> error) => 
            Clone(() => _method.Decorate(transform, new LambdaFilter<TR, TFilteringError>(validator, error)));

        public IFlow<T, TFilteringError> Validate(Func<T, bool> validator, Func<TFilteringError> error) => 
            Clone(() => _method.Decorate(x => x, new LambdaFilter<T, TFilteringError>(validator, error)));

        public IFlow<T, TFilteringError> Validate<TR>(Func<T, TR> transform, IFilter<TR, TFilteringError> filter) => 
            Clone(() => _method.Decorate(transform, filter));

        public IFlow<T, TFilteringError> Validate(IFilter<T, TFilteringError> filter) => 
            Clone(() => _method.Decorate(x => x, filter));

        public IValidatedVerified<T, TFilteringError> Verify<TR>(Func<T, TR> transform, Func<TR, bool> check, Func<TFilteringError> error) => 
            Clone(() => _method.Decorate(transform, new LambdaFilter<TR, TFilteringError>(check, error)));
        
        public IValidatedVerified<T, TFilteringError> Verify(Func<T, bool> check, Func<TFilteringError> error) => 
            Clone(() => _method.Decorate(x => x, new LambdaFilter<T, TFilteringError>(check, error)));

        public IValidatedVerified<T, TFilteringError> Verify(IFilter<T, TFilteringError> filter) => 
            Clone(() => _method.Decorate(x => x, filter));

        public IValidatedVerified<T, TFilteringError> Verify<TR>(Func<T, TR> transform, IFilter<TR, TFilteringError> filter) => 
            Clone(() => _method.Decorate(transform, filter));

        public IValidatedVerified<TR, TFilteringError> Apply<TR>(Func<T, TR> apply) =>
            Clone(() => _method.Decorate(state => apply(state.Result)));
        
        public IValidatedVerified<T, TFilteringError> Raise(Func<T, IEvent> func) =>
            Clone(() => _method.Decorate(state =>
            {
                state.EventReceiver.Receive(func(state.Result));
                return state.Result;
            }));

        public IPipeline<TFilteringError> Finalize(Action<T> execution) => 
            new Pipeline<TFilteringError>(() => _method.Decorate(state => execution(state.Result)));

        public IProjectablePipeline<TR, TFilteringError> Finalize<TR>(Func<T, TR> execution) => 
            new Pipeline<TR, TFilteringError>(() => _method.Decorate(state => execution(state.Result)));

        
        private Step<TR, TFilteringError> Clone<TR>(Func<IState<TR, TFilteringError>> method) => new Step<TR, TFilteringError>(method);
    }
}
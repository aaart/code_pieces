using System;

namespace PipeSharp
{
    public class Step<T, TFilteringError> : INotifyingFlow<T, TFilteringError>, ICheckedAndCheckable<T, TFilteringError>
    {
        private readonly Action _onDoing;
        private readonly Action _onDone;
        private readonly Func<IState<T, TFilteringError>> _method;
        
        public Step(Func<IState<T, TFilteringError>> method, Action onDoing = null, Action onDone = null)
        {
            _method = method;
            _onDoing = onDoing ?? (() => { });
            _onDone = onDone ?? (() => { });
        }

        public IFlow<T, TFilteringError> Check<TR>(Func<T, TR> transform, Func<TR, bool> validator, Func<TFilteringError> error) => 
            Clone(() => _method.Decorate(transform, new LambdaFilter<TR, TFilteringError>(validator, error)));

        public IFlow<T, TFilteringError> Check(Func<T, bool> validator, Func<TFilteringError> error) => 
            Clone(() => _method.Decorate(x => x, new LambdaFilter<T, TFilteringError>(validator, error)));

        public IFlow<T, TFilteringError> Check<TR>(Func<T, TR> transform, IFilter<TR, TFilteringError> filter) => 
            Clone(() => _method.Decorate(transform, filter));

        public IFlow<T, TFilteringError> Check(IFilter<T, TFilteringError> filter) => 
            Clone(() => _method.Decorate(x => x, filter));

        public ICheckedAndCheckable<TR, TFilteringError> Apply<TR>(Func<T, TR> apply) =>
            Clone(() => _method.Decorate(state => apply(state.Result), _onDoing, _onDone));
        
        public ICheckedAndCheckable<T, TFilteringError> Raise<TEvent>(Func<T, TEvent> func) =>
            Clone(() => _method.Decorate(state =>
            {
                state.EventReceiver.Receive(func(state.Result));
                return state.Result;
            }, () => { }, () => { }));

        public IPipeline<TFilteringError> Finalize(Action<T> execution) => 
            new Pipeline<TFilteringError>(() => _method.Decorate(state => execution(state.Result), _onDoing, _onDone));

        public IProjectablePipeline<TR, TFilteringError> Finalize<TR>(Func<T, TR> execution) => 
            new Pipeline<TR, TFilteringError>(() => _method.Decorate(state => execution(state.Result), _onDoing, _onDone));

        
        private Step<TR, TFilteringError> Clone<TR>(Func<IState<TR, TFilteringError>> method) => new Step<TR, TFilteringError>(method, _onDoing, _onDone);
    }
}
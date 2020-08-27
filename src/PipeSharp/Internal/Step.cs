using System;

namespace PipeSharp.Internal
{
    public class Step<T, TError> : INotifyingFlow<T, TError>, ICheckedAndCheckable<T, TError>
    {
        private readonly Action _onDoing;
        private readonly Action _onDone;
        private readonly Func<IState<T, TError>> _method;
        
        public Step(Func<IState<T, TError>> method, Action onDoing = null, Action onDone = null)
        {
            _method = method;
            _onDoing = onDoing ?? (() => { });
            _onDone = onDone ?? (() => { });
        }

        public IFlow<T, TError> Check<TR>(Func<T, TR> transform, Func<TR, bool> validator, Func<TError> error) => 
            Clone(() => _method.Decorate(transform, new LambdaFilter<TR, TError>(validator, error)));

        public IFlow<T, TError> Check(Func<T, bool> validator, Func<TError> error) => 
            Clone(() => _method.Decorate(x => x, new LambdaFilter<T, TError>(validator, error)));

        public IFlow<T, TError> Check<TR>(Func<T, TR> transform, IFilter<TR, TError> filter) => 
            Clone(() => _method.Decorate(transform, filter));

        public IFlow<T, TError> Check(IFilter<T, TError> filter) => 
            Clone(() => _method.Decorate(x => x, filter));

        public ICheckedAndCheckable<TR, TError> Apply<TR>(Func<T, TR> apply) =>
            Clone(() => _method.Decorate(state => apply(state.StepResult), _onDoing, _onDone));
        
        public ICheckedAndCheckable<T, TError> Raise<TEvent>(Func<T, TEvent> func) =>
            Clone(() => _method.Decorate(state =>
            {
                state.Receive(func(state.StepResult));
                return state.StepResult;
            }, () => { }, () => { }));

        public IPipeline<TError> Finalize(Action<T> execution) => 
            new Pipeline<TError>(() => _method.Decorate(state => execution(state.StepResult), _onDoing, _onDone));

        public IProjectablePipeline<TR, TError> Finalize<TR>(Func<T, TR> execution) => 
            new Pipeline<TR, TError>(() => _method.Decorate(state => execution(state.StepResult), _onDoing, _onDone));

        
        private Step<TR, TError> Clone<TR>(Func<IState<TR, TError>> method) => new Step<TR, TError>(method, _onDoing, _onDone);
    }
}
using System;
using Microsoft.Extensions.Logging;

namespace PipeSharp.Internal
{
    public class Step<T, TError> : INotifyingFlow<T, TError>, ICheckedAndCheckable<T, TError>
    {
        private readonly Action _onDoing;
        private readonly Action _onDone;
        private readonly Func<IState<T, TError>> _method;
        private readonly Action<Exception, ILogger> _exceptionHandler;

        public Step(Func<IState<T, TError>> method, Action onDoing, Action onDone, Action<Exception, ILogger> exceptionHandler)
        {
            _method = method;
            _onDoing = onDoing ?? (() => { });
            _onDone = onDone ?? (() => { });
            _exceptionHandler = exceptionHandler ?? ((ex, logger) => { });
        }

        public IFlow<T, TError> Check<TR>(Func<T, TR> transform, Func<TR, bool> validator, Func<TError> error) => 
            Clone(() => _method.Decorate(transform, new LambdaFilter<TR, TError>(validator, error), _exceptionHandler));

        public IFlow<T, TError> Check(Func<T, bool> validator, Func<TError> error) => 
            Clone(() => _method.Decorate(x => x, new LambdaFilter<T, TError>(validator, error), _exceptionHandler));

        public IFlow<T, TError> Check<TR>(Func<T, TR> transform, IFilter<TR, TError> filter) => 
            Clone(() => _method.Decorate(transform, filter, _exceptionHandler));

        public IFlow<T, TError> Check(IFilter<T, TError> filter) => 
            Clone(() => _method.Decorate(x => x, filter, _exceptionHandler));

        public ICheckedAndCheckable<TR, TError> Apply<TR>(Func<T, TR> apply) =>
            Clone(() => _method.Decorate(state => apply(state.StepResult), _onDoing, _onDone, _exceptionHandler));
        
        public ICheckedAndCheckable<T, TError> Raise<TEvent>(Func<T, TEvent> func) =>
            Clone(() => _method.Decorate(state =>
            {
                state.Receive(func(state.StepResult));
                return state.StepResult;
            }, () => { }, () => { }, (ex, logger) => { }));

        public IPipeline<TError> Finalize(Action<T> execution) => 
            new Pipeline<TError>(() => _method.Decorate(state => execution(state.StepResult), _onDoing, _onDone, _exceptionHandler), _exceptionHandler);

        public IProjectablePipeline<TR, TError> Finalize<TR>(Func<T, TR> execution) => 
            new Pipeline<TR, TError>(() => _method.Decorate(state => execution(state.StepResult), _onDoing, _onDone, _exceptionHandler), _exceptionHandler);

        
        private Step<TR, TError> Clone<TR>(Func<IState<TR, TError>> method) => new Step<TR, TError>(method, _onDoing, _onDone, _exceptionHandler);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PipeSharp.Internal;

namespace PipeSharp
{
    public class StandardBuilder : IFlowBuilder
    {
        public StandardBuilder()
            : this(NullLogger.Instance)
        {
        }

        public StandardBuilder(ILogger logger)
        {
            Logger = logger;
        }

        protected ILogger Logger { get; }

        public IFlowBuilder<TError> UseErrorType<TError>(Func<Exception, TError> mapper, params Func<Exception, TError>[] mappers)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            
            return new StandardBuilder<TError>(Logger, new[] { mapper }.Concat(mappers));
        }
    }

    internal class StandardBuilder<TError> : StandardBuilder, IFlowBuilderWithEventSubscriptionEnabled<TError>
    {
        private readonly IEnumerable<Func<Exception, TError>> _exceptionToErrorMappers;
        private ISubscription _subscription = new OutNullSubscription();

        internal StandardBuilder(ILogger logger, IEnumerable<Func<Exception, TError>> exceptionToErrorMappers)
            : base(logger)
        {
            _exceptionToErrorMappers = exceptionToErrorMappers;
        }

        protected List<Action> OnDoingMethods { get; } = new List<Action>();
        protected List<Action> OnDoneMethods { get; } = new List<Action>();
        protected List<Action<Exception, ILogger>> ExceptionHandlers { get; } = new List<Action<Exception, ILogger>>();

        public IFlowBuilder<TError> OnChanging(Action onDoing)
        {
            OnDoingMethods.Add(onDoing);
            return this;
        }

        public IFlowBuilder<TError> OnChanged(Action onDone)
        {
            OnDoneMethods.Add(onDone);
            return this;
        }

        public IFlowBuilderWithEventSubscriptionEnabled<TError> EnableEventSubscription(ISubscription subscription)
        {
            _subscription = subscription;
            return this;
        }

        public IFlowBuilderWithEventSubscriptionEnabled<TError> HandleException(Action<Exception, ILogger> handler)
        {
            ExceptionHandlers.Add(handler);
            return this;
        }

        public IFlow<T, TError> For<T>(T target) => CreateFirstStep(target);

        INotifyingFlow<T, TError> IFlowBuilderWithEventSubscriptionEnabled<TError>.For<T>(T target) => CreateFirstStep(target);
        IFlowBuilder<TError> IFlowBuilder<TError>.HandleException(Action<Exception, ILogger> handler) => HandleException(handler);
        
        private Step<T, TError> CreateFirstStep<T>(T target)
        {
            return new Step<T, TError>(
                () => new State<T, TError>(target, new StateData<TError>(Logger, _subscription.Subscribe())),
                Combine(OnDoingMethods),
                Combine(OnDoneMethods),
                Combine(ExceptionHandlers),
                _exceptionToErrorMappers);
        }

        private static Action Combine(List<Action> actions) => () =>
        {
            foreach (Action action in actions)
            {
                action();
            }
        };

        private static Action<Exception, ILogger> Combine(List<Action<Exception, ILogger>> actions) => (ex, logger) =>
        {
            foreach (Action<Exception, ILogger> action in actions)
            {
                action(ex, logger);
            }
        };
    }
}
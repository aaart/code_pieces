using System;
using System.Collections.Generic;
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
        
        public IFlowBuilder<TError> UseErrorType<TError>() => new StandardBuilder<TError>(Logger);
    }

    internal class StandardBuilder<TError> : StandardBuilder, IFlowBuilderWithEventSubscriptionEnabled<TError>
    {
        private ISubscription _subscription = new OutNullSubscription();

        internal StandardBuilder(ILogger logger)
            : base(logger)
        {
        }


        protected List<Action> OnDoingMethods { get; } = new List<Action>();
        protected List<Action> OnDoneMethods { get; } = new List<Action>();
        protected List<Action<Exception, ILogger>> ExceptionHandlers { get; } = new List<Action<Exception, ILogger>>();
        protected List<Func<Exception, TError>> ExceptionToErrorMappers { get; } = new List<Func<Exception, TError>>();
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

        public IFlowBuilderWithEventSubscriptionEnabled<TError> MapExceptionToErrorOnDeconstruct(Func<Exception, TError> map)
        {
            ExceptionToErrorMappers.Add(map);
            return this;
        }

        public IFlow<T, TError> For<T>(T target) => CreateFirstStep(target);

        INotifyingFlow<T, TError> IFlowBuilderWithEventSubscriptionEnabled<TError>.For<T>(T target) => CreateFirstStep(target);
        IFlowBuilder<TError> IFlowBuilder<TError>.HandleException(Action<Exception, ILogger> handler) => HandleException(handler);
        IFlowBuilder<TError> IFlowBuilder<TError>.MapExceptionToErrorOnDeconstruct(Func<Exception, TError> map) => MapExceptionToErrorOnDeconstruct(map);
        
        private Step<T, TError> CreateFirstStep<T>(T target) =>
            new Step<T, TError>(
                () => new State<T, TError>(target, new StateData<TError>(Logger, _subscription.Subscribe())),
                Combine(OnDoingMethods),
                Combine(OnDoneMethods),
                Combine(ExceptionHandlers),
                ExceptionToErrorMappers);

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
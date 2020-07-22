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
        
        public IFlowBuilder<TFilteringError> WithFilteringError<TFilteringError>() => new StandardBuilder<TFilteringError>(Logger);
    }

    public class StandardBuilder<TFilteringError> : StandardBuilder, IFlowBuilderWithEventSubscriptionEnabled<TFilteringError>
    {
        private ISubscription _subscription = new OutNullSubscription();

        internal StandardBuilder(ILogger logger)
            : base(logger)
        {
        }


        protected List<Action> OnDoingMethods { get; } = new List<Action>();
        protected List<Action> OnDoneMethods { get; } = new List<Action>();
        public IFlowBuilder<TFilteringError> OnChanging(Action onDoing)
        {
            OnDoingMethods.Add(onDoing);
            return this;
        }

        public IFlowBuilder<TFilteringError> OnChanged(Action onDone)
        {
            OnDoneMethods.Add(onDone);
            return this;
        }

        public IFlowBuilderWithEventSubscriptionEnabled<TFilteringError> EnableEventSubscription(ISubscription subscription)
        {
            _subscription = subscription;
            return this;
        }

        public IFlow<T, TFilteringError> For<T>(T target) => CreateFirstStep(target);

        INotifyingFlow<T, TFilteringError> IFlowBuilderWithEventSubscriptionEnabled<TFilteringError>.For<T>(T target) => CreateFirstStep(target);

        private Step<T, TFilteringError> CreateFirstStep<T>(T target) =>
            new Step<T, TFilteringError>(
                () => new State<T, TFilteringError>(target, new StateData<TFilteringError>(Logger, _subscription.Subscribe())),
                Combine(OnDoingMethods),
                Combine(OnDoneMethods));

        private static Action Combine(List<Action> actions) => () =>
        {
            foreach (Action action in actions)
            {
                action();
            }
        };
    }
}
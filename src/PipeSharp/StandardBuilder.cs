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

        protected StandardBuilder(ILogger logger, List<Action> onDoing, List<Action> onDone)
            : this(logger)
        {
            OnDoingMethods.AddRange(onDoing);
            OnDoneMethods.AddRange(onDone);
        }

        protected List<Action> OnDoingMethods { get; } = new List<Action>();
        protected List<Action> OnDoneMethods { get; } = new List<Action>();
        protected ILogger Logger { get; }

        public IFlowBuilder OnDoing(Action onDoing)
        {
            OnDoingMethods.Add(onDoing);
            return this;
        }

        public IFlowBuilder OnDone(Action onDone)
        {
            OnDoneMethods.Add(onDone);
            return this;
        }

        public IFlowBuilder<TFilteringError> WithFilteringError<TFilteringError>() => new StandardBuilder<TFilteringError>(Logger, OnDoingMethods, OnDoneMethods);
    }

    public class StandardBuilder<TFilteringError> : StandardBuilder, IFlowBuilder<TFilteringError>
    {
        internal StandardBuilder(ILogger logger, List<Action> onDoing, List<Action> onDone)
            : base(logger, onDoing, onDone)
        {
        }

        public new IFlowBuilder<TFilteringError> OnDoing(Action onDoing) => (IFlowBuilder<TFilteringError>)base.OnDoing(onDoing);

        public new IFlowBuilder<TFilteringError> OnDone(Action onDone) => (IFlowBuilder<TFilteringError>)base.OnDone(onDone);

        public IFlowPreDefined<TFilteringError> WithoutEvents() =>
            new StandardFlowPreDefined<TFilteringError>(Logger, Combine(OnDoingMethods), Combine(OnDoneMethods));

        public INotifyingFlowPreDefined<TFilteringError> WithEvents(IEventReceiverFactory eventReceiverFactory) =>
            new NotifyingFlowPreDefined<TFilteringError>(Logger, eventReceiverFactory, Combine(OnDoingMethods), Combine(OnDoneMethods));

        private static Action Combine(List<Action> actions) => () =>
            {
                foreach (Action action in actions)
                {
                    action();
                }
            };
    }
}
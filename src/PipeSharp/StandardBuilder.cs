using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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

        public IFlowBuilder<TFilteringError> WithFilteringError<TFilteringError>() => new StandardBuilder<TFilteringError>();
    }

    public class StandardBuilder<TFilteringError> : StandardBuilder, IFlowBuilder<TFilteringError>
    {
        internal StandardBuilder()
        {
        }

        public new IFlowBuilder<TFilteringError> OnDoing(Action onDoing) => (IFlowBuilder<TFilteringError>)base.OnDoing(onDoing);

        public new IFlowBuilder<TFilteringError> OnDone(Action onDone) => (IFlowBuilder<TFilteringError>)base.OnDone(onDone);

        public IFlowPreDefined<TFilteringError> WithoutEvents() =>
            new StandardFlowPreDefined<TFilteringError>(Logger, Combine(OnDoingMethods), Combine(OnDoneMethods));

        public INotifyingFlowPreDefined<TFilteringError> WithEvents(IEventReceiverFactory eventReceiverFactory) =>
            new NotifyingFlowPreDefined<TFilteringError>(Logger, eventReceiverFactory, Combine(OnDoingMethods), Combine(OnDoneMethods));

        private static Action Combine(List<Action> actions) => () => actions.ForEach(x => x());
    }
}
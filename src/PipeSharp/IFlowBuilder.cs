using System;

namespace PipeSharp
{
    public interface IFlowBuilder
    {
        IFlowBuilder<TFilteringError> WithFilteringError<TFilteringError>();
    }

    public interface IFlowBuilder<TFilteringError> : IFlowBuilder
    {
        IFlowBuilder<TFilteringError> OnDoing(Action onDoing);
        IFlowBuilder<TFilteringError> OnDone(Action onDone);
        IFlowBuilderWithEventsApplied<TFilteringError> WithEvents(IEventReceiverFactory eventReceiverFactory);
        IFlow<T, TFilteringError> For<T>(T target);
    }
}
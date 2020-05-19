using System;

namespace PipeSharp
{
    public interface IFlowBuilder
    {
        IFlowBuilder<TFilteringError> WithFilteringError<TFilteringError>();
    }

    public interface IFlowBuilder<TFilteringError> : IOnChangingOnChangedApplier<TFilteringError>
    {
        IFlowBuilderWithEventsApplied<TFilteringError> WithEvents(IEventReceiverFactory eventReceiverFactory);
        IFlow<T, TFilteringError> For<T>(T target);
    }
}
using System;

namespace PipeSharp
{
    public interface IFlowBuilder
    {
        IFlowBuilder<TFilteringError> WithFilteringError<TFilteringError>();
    }

    public interface IFlowBuilder<TFilteringError> : IOnChangingOnChangedApplier<TFilteringError>
    {
        IFlowBuilderWithEventSubscriptionEnabled<TFilteringError> EnableEventSubscription(ISubscription subscription);
        IFlow<T, TFilteringError> For<T>(T target);
    }
}
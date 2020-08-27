using System;

namespace PipeSharp
{
    public interface IFlowBuilder
    {
        IFlowBuilder<TError> WithFilteringError<TError>();
    }

    public interface IFlowBuilder<TFilteringError> : IOnChangingOnChangedApplier<TFilteringError>
    {
        IFlowBuilderWithEventSubscriptionEnabled<TFilteringError> EnableEventSubscription(ISubscription subscription);
        IFlow<T, TFilteringError> For<T>(T target);
    }
}
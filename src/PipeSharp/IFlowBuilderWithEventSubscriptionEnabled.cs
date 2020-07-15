using System;

namespace PipeSharp
{
    public interface IFlowBuilderWithEventSubscriptionEnabled<TFilteringError> : IFlowBuilder<TFilteringError>
    {
        new INotifyingFlow<T, TFilteringError> For<T>(T target);
    }
}
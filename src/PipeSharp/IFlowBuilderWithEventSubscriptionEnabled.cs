using System;

namespace PipeSharp
{
    public interface IFlowBuilderWithEventSubscriptionEnabled<TError> : IFlowBuilder<TError>
    {
        new INotifyingFlow<T, TError> For<T>(T target);
    }
}
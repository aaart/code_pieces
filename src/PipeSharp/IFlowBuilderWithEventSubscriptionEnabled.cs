using System;
using Microsoft.Extensions.Logging;

namespace PipeSharp
{
    public interface IFlowBuilderWithEventSubscriptionEnabled<TError> : IFlowBuilder<TError>
    {
        new INotifyingFlow<T, TError> For<T>(T target);
        new IFlowBuilderWithEventSubscriptionEnabled<TError> HandleException(Action<Exception, ILogger> handler);
    }
}
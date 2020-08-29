using System;
using Microsoft.Extensions.Logging;

namespace PipeSharp
{
    public interface IFlowBuilder
    {
        IFlowBuilder<TError> UseErrorType<TError>();
    }

    public interface IFlowBuilder<TError> : IOnChangingOnChangedApplier<TError>
    {
        IFlowBuilderWithEventSubscriptionEnabled<TError> EnableEventSubscription(ISubscription subscription);
        IFlowBuilder<TError> HandleException(Action<Exception, ILogger> handler);
        IFlowBuilder<TError> MapExceptionToErrorOnDeconstruct(Func<Exception, TError> map);
        IFlow<T, TError> For<T>(T target);
    }
}
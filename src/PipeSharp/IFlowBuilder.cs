using System;
using Microsoft.Extensions.Logging;

namespace PipeSharp
{
    public interface IFlowBuilder
    {
        IFlowBuilder<TError> UseErrorType<TError>(Func<Exception, TError> mapper, params Func<Exception, TError>[] mappers);
    }

    public interface IFlowBuilder<TError> : IOnChangingOnChangedApplier<TError>
    {
        IFlowBuilderWithEventSubscriptionEnabled<TError> EnableEventSubscription(ISubscription subscription);
        IFlowBuilder<TError> HandleException(Action<Exception, ILogger> handler);
        IFlow<T, TError> For<T>(T target);
    }
}
using System;

namespace PipeSharp
{
    public interface IFlowBuilderWithEventsApplied<TFilteringError> : IFlowBuilder<TFilteringError>
    {
        new INotifyingFlow<T, TFilteringError> For<T>(T target);
    }
}
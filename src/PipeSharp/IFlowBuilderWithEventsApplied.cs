using System;

namespace PipeSharp
{
    public interface IFlowBuilderWithEventsApplied<TFilteringError>
    {
        INotifyingFlow<T, TFilteringError> For<T>(T target);
    }
}
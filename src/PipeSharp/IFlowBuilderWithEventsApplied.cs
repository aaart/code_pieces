using System;

namespace PipeSharp
{
    public interface IFlowBuilderWithEventsApplied<TFilteringError> : IOnChangingOnChangedApplier<TFilteringError>
    {
        INotifyingFlow<T, TFilteringError> For<T>(T target);
    }
}
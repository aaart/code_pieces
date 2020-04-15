using System;

namespace PipeSharp
{
    public interface IOnChangingOnChangedApplier<TFilteringError>
    {
        IFlowBuilder<TFilteringError> OnChanging(Action onDoing);
        IFlowBuilder<TFilteringError> OnChanged(Action onDone);
    }
}
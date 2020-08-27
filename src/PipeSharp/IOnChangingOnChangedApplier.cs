using System;

namespace PipeSharp
{
    public interface IOnChangingOnChangedApplier<TError>
    {
        IFlowBuilder<TError> OnChanging(Action onDoing);
        IFlowBuilder<TError> OnChanged(Action onDone);
    }
}
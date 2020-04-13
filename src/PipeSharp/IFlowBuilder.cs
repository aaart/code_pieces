using System;

namespace PipeSharp
{
    public interface IFlowBuilder
    {
        IFlowBuilder OnDoing(Action onDoing);
        IFlowBuilder OnDone(Action onDone);
        IFlowBuilder<TFilteringError> WithFilteringError<TFilteringError>();
    }

    public interface IFlowBuilder<TFilteringError> : IFlowBuilder
    {
        new IFlowBuilder<TFilteringError> OnDoing(Action onDoing);
        new IFlowBuilder<TFilteringError> OnDone(Action onDone);
        IFlowPreDefined<TFilteringError> WithoutEvents();
        INotifyingFlowPreDefined<TFilteringError> WithEvents(IEventReceiverFactory eventReceiverFactory);
    }
}
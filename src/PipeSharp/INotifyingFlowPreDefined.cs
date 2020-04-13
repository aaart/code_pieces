using System;

namespace PipeSharp
{
    public interface INotifyingFlowPreDefined<TFilteringError>
    {
        INotifyingFlow<T, TFilteringError> For<T>(T target);
    }
}
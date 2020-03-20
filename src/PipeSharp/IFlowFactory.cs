using System;

namespace PipeSharp
{
    public interface IFlowFactory<TFilteringError>
    {
        IFlow<T, TFilteringError> For<T>(T target);
        IFlow<T, TFilteringError> For<T>(T target, Action onDoing, Action onDone);
    }
}
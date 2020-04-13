using System;

namespace PipeSharp
{
    public interface IFlowPreDefined<TFilteringError>
    {
        IFlow<T, TFilteringError> For<T>(T target);
    }
}
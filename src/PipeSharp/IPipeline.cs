using System;

namespace PipeSharp
{
    public interface IPipeline<TFilteringError>
    {
        IPipelineResult<TFilteringError> Sink();
    }

    public interface IPipeline<T, TFilteringError> : IPipeline<TFilteringError>
    {
        new IPipelineResult<T, TFilteringError> Sink();
    }
}
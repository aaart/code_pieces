using System;

namespace PipeSharp
{
    public interface IPipeline<TFilteringError>
    {
        (IPipelineResult, Exception, TFilteringError[]) Sink();
    }

    public interface IPipeline<T, TFilteringError> : IPipeline<TFilteringError>
    {
        new (IPipelineResult<T>, Exception, TFilteringError[]) Sink();
    }
}
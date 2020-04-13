using System;

namespace PipeSharp
{
    public interface IPipeline<TFilteringError>
    {
        IPipelineSummary<TFilteringError> Sink();
    }

    public interface IPipeline<T, TFilteringError> : IPipeline<TFilteringError>
    {
        new IPipelineSummary<T, TFilteringError> Sink();
    }
}
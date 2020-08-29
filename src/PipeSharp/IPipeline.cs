using System;

namespace PipeSharp
{
    public interface IPipeline<TError>
    {
        IPipelineSummary<TError> Sink();
    }

    public interface IPipeline<T, TError> : IPipeline<TError>
    {
        new IPipelineSummary<T, TError> Sink();
    }
}
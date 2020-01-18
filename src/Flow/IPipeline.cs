using System;

namespace Flow
{
    public interface IPipeline
    {
        IPipelineResult Sink();
    }

    public interface IPipeline<out T> : IPipeline
    {
        new IPipelineResult<T> Sink();
    }
}
using System;

namespace Flow
{
    public interface ISink
    {
        IPipelineResult Execute(IPipeline pipeline);
    }


    public interface ISink<T> : ISink
    {
        IPipelineResult<T> Execute(IPipeline<T> pipeline);
    }
}
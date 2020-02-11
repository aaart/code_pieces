using System;

namespace PipeSharp
{
    public interface IPipeline<TFilteringError>
    {
        (IResult, Exception, TFilteringError[]) Sink();
    }

    public interface IPipeline<T, TFilteringError> : IPipeline<TFilteringError>
    {
        new (IResult<T>, Exception, TFilteringError[]) Sink();
    }
}
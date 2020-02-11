using System;

namespace PipeSharp
{
    public interface IPipelineResult<TFilteringError>
    {
        IResult Result { get; }
        Exception Exception { get; }
        TFilteringError[] FilteringErrors { get; }

        void Deconstruct(out IResult result, out Exception exception, out TFilteringError[] errors);
    }

    public interface IPipelineResult<T, TFilteringError> : IPipelineResult<TFilteringError>
    {
        new IResult<T> Result { get; }
        void Deconstruct(out IResult<T> result, out Exception exception, out TFilteringError[] errors);
    }
}
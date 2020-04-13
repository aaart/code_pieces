using System;

namespace PipeSharp
{
    public interface IPipelineSummary<TFilteringError>
    {
        IResult Result { get; }
        Exception Exception { get; }
        TFilteringError[] FilteringErrors { get; }

        void Deconstruct(out IResult result, out Exception exception, out TFilteringError[] errors);
    }

    public interface IPipelineSummary<T, TFilteringError> : IPipelineSummary<TFilteringError>
    {
        new IResult<T> Result { get; }
        void Deconstruct(out IResult<T> result, out Exception exception, out TFilteringError[] errors);
    }
}
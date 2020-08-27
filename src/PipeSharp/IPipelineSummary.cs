using System;

namespace PipeSharp
{
    public interface IPipelineSummary<TError>
    {
        IResult Result { get; }
        Exception Exception { get; }
        TError[] Errors { get; }

        void Deconstruct(out IResult result, out Exception exception, out TError[] errors);
    }

    public interface IPipelineSummary<T, TError> : IPipelineSummary<TError>
    {
        new IResult<T> Result { get; }
        void Deconstruct(out IResult<T> result, out Exception exception, out TError[] errors);
    }
}
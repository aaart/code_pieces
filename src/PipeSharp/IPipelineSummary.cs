using System;

namespace PipeSharp
{
    public interface IPipelineSummary<TError>
    {
        TError[] Errors { get; }
        
        void Deconstruct(out TError[] errors);
    }

    public interface IPipelineSummary<T, TError> : IPipelineSummary<TError>
    {
        T Value { get; }
        void Deconstruct(out T value, out TError[] errors);
    }
}
using System;

namespace PipeSharp
{
    public interface IPipelineSummary<TError>
    {
        bool Failed { get; }
        Exception Exception { get; }
        TError[] Errors { get; }

        void Deconstruct(out bool failed, out TError[] errors);
        void Deconstruct(out bool failed, out Exception exception, out TError[] errors);
    }

    public interface IPipelineSummary<T, TError> : IPipelineSummary<TError>
    {
        T Value { get; }
        void Deconstruct(out bool failed, out T value, out TError[] errors);
        void Deconstruct(out bool failed, out T value, out Exception exception, out TError[] errors);
    }
}
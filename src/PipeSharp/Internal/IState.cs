using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace PipeSharp.Internal
{

    public interface IState<TError> : ILogger
    {
        bool Broken { get; }
        bool Invalid { get; }
        IState<TError> Fail();
        IState<TError> Fail(Exception exception);
        IState<TError> Invalidate(TError filteringError);
        IEnumerable<TError> FilteringErrors { get; }
        Exception Exception { get; }
        void Receive<TEvent>(TEvent e);
        void Done();
    }

    public interface IState<out T, TError> : IState<TError>
    {
        IState<TError> Next();
        IState<TR, TError> Next<TR>(TR result);
        IState<TR, TError> Fail<TR>();
        IState<TR, TError> Fail<TR>(Exception exception);
        new IState<T, TError> Invalidate(TError filteringError);
        T StepResult { get; }
    }
}
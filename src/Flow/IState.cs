using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace PipeSharp
{

    public interface IState<TFilteringError> : ILogger
    {
        bool Broken { get; }
        bool Invalid { get; }
        IState<TFilteringError> Fail();
        IState<TFilteringError> Fail(Exception exception);
        IState<TFilteringError> Invalidate(TFilteringError filteringError);
        IEnumerable<TFilteringError> FilteringErrors { get; }
        Exception Exception { get; }
        IEventReceiver EventReceiver { get; }
        void Done();
    }

    public interface IState<out T, TFilteringError> : IState<TFilteringError>
    {
        IState<TFilteringError> Next();
        IState<TR, TFilteringError> Next<TR>(TR result);
        IState<TR, TFilteringError> Fail<TR>();
        IState<TR, TFilteringError> Fail<TR>(Exception exception);
        new IState<T, TFilteringError> Invalidate(TFilteringError filteringError);
        T Result { get; }
    }
}
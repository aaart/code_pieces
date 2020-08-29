using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace PipeSharp.Internal
{
    public class State<TError> : IState<TError>
    {
        protected internal State(StateData<TError> stateData)
        {
            Data = stateData;
        }
        protected StateData<TError> Data { get; }

        public IEnumerable<TError> FilteringErrors => Data.FilteringErrors;
        public Exception Exception => Data.Exception;
        public bool Broken => Data.Broken;
        public bool Invalid => Data.Invalid;

        public IState<TError> Fail()
        {
            Data.Broken = true;
            return this;
        }

        public IState<TError> Fail(Exception exception)
        {
            Data.Exception = exception;
            Data.Broken = true;
            return this;
        }

        public IState<TError> Invalidate(TError filteringError)
        {
            Data.FilteringErrors.Add(filteringError);
            Data.Invalid = true;
            return this;
        }

        public void Receive<TEvent>(TEvent e)
        {
            Data.ActiveSubscription.Receive(e);
        }

        public void Done()
        {
            Data.Dispose();
            this.LogDebug("All operations done.");
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) => 
            Data.Logger.Log(logLevel, eventId, state, exception, formatter);

        public bool IsEnabled(LogLevel logLevel) => Data.Logger.IsEnabled(logLevel);

        public IDisposable BeginScope<TState>(TState state) => Data.Logger.BeginScope(state);
    }

    public class State<T, TFilteringError> : State<TFilteringError>, IState<T, TFilteringError>
    {
        public State(T result, StateData<TFilteringError> stateData)
            : base(stateData)
        {
            StepResult = result;
        }

        public T StepResult { get; }
        
        public IState<TR, TFilteringError> Next<TR>(TR result) => new State<TR, TFilteringError>(result, Data);

        public IState<TR, TFilteringError> Fail<TR>()
        {
            Data.Broken = true;
            return new State<TR, TFilteringError>(default, Data);
        }

        public IState<TR, TFilteringError> Fail<TR>(Exception exception)
        {
            Data.Exception = exception;
            Data.Broken = true;
            return new State<TR, TFilteringError>(default, Data);
        }

        public new IState<T, TFilteringError> Invalidate(TFilteringError filteringError)
        {
            Data.FilteringErrors.Add(filteringError);
            Data.Invalid = true;
            return this;
        }

        public IState<TFilteringError> Next() => new State<TFilteringError>(Data);
    }
}
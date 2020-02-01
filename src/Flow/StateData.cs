using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Flow
{
    public class StateData<TFilteringError> : IDisposable
    {
        public StateData(ILogger  logger, IEventReceiver eventReceiver)
            : this(logger, eventReceiver, new List<TFilteringError>(), null, false)
        {
        }

        protected  internal StateData(
            ILogger logger,
            IEventReceiver eventReceiver,
            List<TFilteringError> filteringErrors, 
            Exception exception, 
            bool broken)
        {
            Logger = logger;
            EventReceiver = eventReceiver;
            FilteringErrors = filteringErrors;
            Exception = exception;
            Broken = broken;
        }
        public ILogger Logger { get; }
        public IEventReceiver EventReceiver { get; }
        public List<TFilteringError> FilteringErrors { get; }
        public Exception Exception { get; set; }
        public bool Broken { get; set; }
        public bool Invalid { get; set; }

        public void Dispose()
        {
            EventReceiver.Dispose();
        }
    }
}
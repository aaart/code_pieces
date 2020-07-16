using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace PipeSharp.Internal
{
    public class StateData<TFilteringError> : IDisposable
    {
        public StateData(ILogger  logger, IActiveSubscription activeSubscription)
            : this(logger, activeSubscription, new List<TFilteringError>(), null, false)
        {
        }

        protected  internal StateData(
            ILogger logger,
            IActiveSubscription activeSubscription,
            List<TFilteringError> filteringErrors, 
            Exception exception, 
            bool broken)
        {
            Logger = logger;
            ActiveSubscription = activeSubscription;
            FilteringErrors = filteringErrors;
            Exception = exception;
            Broken = broken;
        }
        public ILogger Logger { get; }
        public IActiveSubscription ActiveSubscription { get; }
        public List<TFilteringError> FilteringErrors { get; }
        public Exception Exception { get; set; }
        public bool Broken { get; set; }
        public bool Invalid { get; set; }

        public void Dispose()
        {
            ActiveSubscription.Dispose();
        }
    }
}
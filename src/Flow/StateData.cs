using System;
using System.Collections.Generic;

namespace Flow
{
    public class StateData : IDisposable
    {
        public StateData(IEventReceiver eventReceiver)
            : this(eventReceiver, new List<IFilteringError>(), null, false)
        {
            
        }

        protected  internal StateData(
            IEventReceiver eventReceiver,
            List<IFilteringError> filteringErrors, 
            Exception exception, 
            bool broken)
        {
            EventReceiver = eventReceiver;
            FilteringErrors = filteringErrors;
            Exception = exception;
            Broken = broken;
        }

        public IEventReceiver EventReceiver { get; }
        public List<IFilteringError> FilteringErrors { get; }
        public Exception Exception { get; set; }
        public bool Broken { get; set; }
        public bool Invalid { get; set; }

        public void Dispose()
        {
            EventReceiver.Dispose();
        }
    }
}
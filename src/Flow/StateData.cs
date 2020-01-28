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
        
        public StateData(IEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, IEventReceiver> onStateDone)
            : this(eventReceiver, new List<IFilteringError>(), null, false)
        {
            
        }

        protected  internal StateData(
            IEventReceiver eventReceiver,
            List<IFilteringError> filteringErrors, 
            Exception exception, 
            bool failed)
        {
            EventReceiver = eventReceiver;
            FilteringErrors = filteringErrors;
            Exception = exception;
            Failed = failed;
        }

        public IEventReceiver EventReceiver { get; }
        public List<IFilteringError> FilteringErrors { get; }
        public Exception Exception { get; set; }
        public bool Failed { get; set; }
        public void Dispose()
        {
            EventReceiver.Dispose();
        }
    }
}
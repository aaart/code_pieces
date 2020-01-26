using System;
using System.Collections.Generic;

namespace Flow
{
    public class StateData<TEventReceiver>
    {
        public StateData(TEventReceiver eventReceiver)
            : this(eventReceiver, (e, er) => { }, new List<IFilteringError>(), null, false)
        {
            
        }
        
        public StateData(TEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, TEventReceiver> onStateDone)
            : this(eventReceiver, onStateDone, new List<IFilteringError>(), null, false)
        {
            
        }

        protected  internal StateData(
            TEventReceiver eventReceiver, 
            Action<IEnumerable<IFilteringError>, TEventReceiver> onStateDone, 
            List<IFilteringError> filteringErrors, 
            Exception exception, 
            bool failed)
        {
            EventReceiver = eventReceiver;
            OnStateDone = onStateDone;
            FilteringErrors = filteringErrors;
            Exception = exception;
            Failed = failed;
        }

        public TEventReceiver EventReceiver { get; }
        public Action<IEnumerable<IFilteringError>, TEventReceiver> OnStateDone { get; }
        public List<IFilteringError> FilteringErrors { get; }
        public Exception Exception { get; set; }
        public bool Failed { get; set; }
    }
}
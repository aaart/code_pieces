using System;
using System.Collections.Generic;

namespace Flow
{
    public class StepState<TEventReceiver>
    {
        public StepState(TEventReceiver eventReceiver)
            : this(eventReceiver, (e, er) => { }, new List<IFilteringError>(), null, false)
        {
            
        }
        
        public StepState(TEventReceiver eventReceiver, Action<IEnumerable<IFilteringError>, TEventReceiver> onStateDone)
            : this(eventReceiver, onStateDone, new List<IFilteringError>(), null, false)
        {
            
        }

        protected  internal StepState(
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
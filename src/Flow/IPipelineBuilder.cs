using System;
using System.Collections.Generic;

namespace Flow
{
    public interface IPipelineBuilder
    {
        IBeginFlow<T> For<T>(T target);
        IBeginFlow<T> For<T, TEventReceiver>(T target, Action<IEnumerable<IFilteringError>, TEventReceiver> onStateDone)
            where TEventReceiver : IEventReceiver, new();
    }
}
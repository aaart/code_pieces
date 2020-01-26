﻿using System;
using System.Collections.Generic;

namespace Flow
{
    public class StandardPipelineBuilder : IPipelineBuilder
    {
        public IBeginFlow<T> For<T>(T target) => 
            new Step<T>(() => new State<T, StandardEventReceiver>(target, new StateData<StandardEventReceiver>(new StandardEventReceiver(), (e, er) => { })));

        public IBeginFlow<T> For<T, TEventReceiver>(T target, Action<IEnumerable<IFilteringError>, TEventReceiver> onStateDone)
            where TEventReceiver : IEventReceiver, new() =>
            new Step<T>(() => new State<T, TEventReceiver>(target, new StateData<TEventReceiver>(new TEventReceiver(), onStateDone)));
    }
}
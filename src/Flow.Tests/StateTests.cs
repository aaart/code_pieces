using System;
using System.Collections.Generic;
using Xunit;

namespace Flow.Tests
{
    public class StateTests
    {
        public class TestingState : State<int, BlackholeEventReceiver>
        {
            public TestingState(Action<IEnumerable<IFilteringError>, BlackholeEventReceiver> onStateDone)
                : base(0, new StateData<BlackholeEventReceiver>(new BlackholeEventReceiver(), onStateDone))
            {
            }

        }

        [Fact]
        public void Flow_ExpectStateDone()
        {
            bool done = false;
            new Step<int>(() => new TestingState((e, er) => done = true)).Finalize(x => { }).Sink();

            Assert.True(done);
        }
    }
}
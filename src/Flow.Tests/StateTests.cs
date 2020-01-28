using System;
using System.Collections.Generic;
using Xunit;

namespace Flow.Tests
{
    public class StateTests
    {
        public class TestingStepResult : StepResult<int, BlackholeEventReceiver>
        {
            public TestingStepResult(Action<IEnumerable<IFilteringError>, BlackholeEventReceiver> onStateDone)
                : base(0, new StepState<BlackholeEventReceiver>(new BlackholeEventReceiver(), onStateDone))
            {
            }

        }

        [Fact]
        public void Flow_ExpectStateDone()
        {
            bool done = false;
            new Step<int>(() => new TestingStepResult((e, er) => done = true)).Finalize(x => { }).Sink();

            Assert.True(done);
        }
    }
}
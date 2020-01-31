using System;
using System.Collections.Generic;
using Flow.Tests.TestUtilities;
using Xunit;

namespace Flow.Tests
{
    public class StateTests
    {
        public class TestingState : State<int, TestingFilteringError>
        {
            public TestingState(TestingEventReceiver eventReceiver)
                : base(0, new StateData<TestingFilteringError>(eventReceiver))
            {
            }

        }

        [Fact]
        public void Flow_ExpectStateDone()
        {
            bool done = false;
            new Step<int, TestingFilteringError>(() => new TestingState(new TestingEventReceiver(() => { }, () => done = true))).Finalize(x => { }).Sink();

            Assert.True(done);
        }
    }
}
using System;
using System.Collections.Generic;
using Flow.Tests.TestUtilities;
using Xunit;

namespace Flow.Tests
{
    public class StateTests
    {
        public class TestingState : State<int>
        {
            public TestingState(TestingEventReceiver eventReceiver)
                : base(0, new StateData(eventReceiver))
            {
            }

        }

        [Fact]
        public void Flow_ExpectStateDone()
        {
            bool done = false;
            new Step<int>(() => new TestingState(new TestingEventReceiver(() => { }, () => done = true))).Finalize(x => { }).Sink();

            Assert.True(done);
        }
    }
}
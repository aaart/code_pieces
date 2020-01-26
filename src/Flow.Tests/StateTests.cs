using System;
using System.Collections.Generic;
using Flow.Tests.TestUtilities;
using Xunit;

namespace Flow.Tests
{
    public class StateTests
    {
        public class TestingState : State<int, StandardEventReceiver>
        {
            public TestingState(Action<IEnumerable<IFilteringError>, StandardEventReceiver> onStateDone)
                : base(0, new StandardEventReceiver(), onStateDone)
            {
            }

        }

        [Fact]
        public void Flow_ExpectStateDisposed()
        {
            bool disposed = false;
            new Step<int>(() => new TestingState((e, er) => disposed = true)).Finalize(x => { }).Sink();

            Assert.True(disposed);
        }
    }
}
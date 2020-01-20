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
            public TestingState(Action<IEnumerable<IError>, IEventReceiver> onDisposing)
                : base(0, new List<IError>(), new StandardEventReceiver(), onDisposing)
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
using Microsoft.Extensions.Logging.Abstractions;
using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public class NotyfingFlowTests_Events
    {
        [Fact]
        public void Flow_WhenTestingEventPublished_ExpectEventReceived1()
        {
            bool received = false;
            new Step<int, TestingFilteringError>(() => new State<int, TestingFilteringError>(default, new StateData<TestingFilteringError>(NullLogger.Instance, new TestingEventReceiver(() => received = true, () => { }))))
                .Raise(x => new TestingEvent())
                .Finalize(x => { })
                .Sink();

            Assert.True(received);
        }

        [Fact]
        public void Flow_WhenTestingEventPublished_ExpectEventReceived2()
        {
            bool received = false;
            new Step<int, TestingFilteringError>(() => new State<int, TestingFilteringError>(default, new StateData<TestingFilteringError>(NullLogger.Instance, new TestingEventReceiver(() => received = true, () => { }))))
                .Raise(x => new TestingEvent())
                .Finalize(x => x)
                .Sink();
            
            Assert.True(received);
        }

        [Fact]
        public void Flow_WhenTestingEventPublished_ExpectEventReceived3()
        {
            bool received = false;
            new Step<int, TestingFilteringError>(() => new State<int, TestingFilteringError>(default, new StateData<TestingFilteringError>(NullLogger.Instance, new TestingEventReceiver(() => received = true, () => { }))))
                .Raise(x => new TestingEvent())
                .Finalize(x => x)
                .Project(x => x)
                .Sink();
            
            Assert.True(received);
        }
    }
}
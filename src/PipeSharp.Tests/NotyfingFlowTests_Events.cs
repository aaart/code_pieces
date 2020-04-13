using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public partial class NotyfingFlowTests
    {
        [Fact]
        public void Flow_WhenTestingEventPublished_ExpectEventReceived1()
        {
            bool received = false;
            INotifyingFlowFactory<TestingFilteringError> factory = 
                new NotifyingFlowFactory<TestingFilteringError>(new TestingEventReceiverFactory(() => received = true, () => { }));

            factory.For(default(int))
                .Raise(x => new TestingEvent())
                .Finalize(x => { })
                .Sink();

            Assert.True(received);
        }

        [Fact]
        public void Flow_WhenTestingEventPublished_ExpectEventReceived2()
        {
            bool received = false;
            INotifyingFlowFactory<TestingFilteringError> factory =
                new NotifyingFlowFactory<TestingFilteringError>(new TestingEventReceiverFactory(() => received = true, () => { }));

            factory.For(default(int))
                .Raise(x => new TestingEvent())
                .Finalize(x => x)
                .Sink();
            
            Assert.True(received);
        }

        [Fact]
        public void Flow_WhenTestingEventPublished_ExpectEventReceived3()
        {
            bool received = false;
            INotifyingFlowFactory<TestingFilteringError> factory =
                new NotifyingFlowFactory<TestingFilteringError>(new TestingEventReceiverFactory(() => received = true, () => { }));

            factory.For(default(int))
                .Raise(x => new TestingEvent())
                .Finalize(x => x)
                .Project(x => x)
                .Sink();
            
            Assert.True(received);
        }
    }
}
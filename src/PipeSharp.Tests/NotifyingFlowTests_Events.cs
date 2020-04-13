using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public partial class NotifyingFlowTests
    {
        [Fact]
        public void Flow_WhenTestingEventPublished_ExpectEventReceived1()
        {
            bool received = false;

            new StandardBuilder()
                .WithFilteringError<TestingFilteringError>()
                .WithEvents(new TestingEventReceiverFactory(() => received = true, () => { }))
                .For(default(int))
                .Raise(x => new TestingEvent())
                .Finalize(x => { })
                .Sink();

            Assert.True(received);
        }

        [Fact]
        public void Flow_WhenTestingEventPublished_ExpectEventReceived2()
        {
            bool received = false;

            new StandardBuilder()
                .WithFilteringError<TestingFilteringError>()
                .WithEvents(new TestingEventReceiverFactory(() => received = true, () => { }))
                .For(default(int))
                .Raise(x => new TestingEvent())
                .Finalize(x => x)
                .Sink();
            
            Assert.True(received);
        }

        [Fact]
        public void Flow_WhenTestingEventPublished_ExpectEventReceived3()
        {
            bool received = false;

            new StandardBuilder()
                .WithFilteringError<TestingFilteringError>()
                .WithEvents(new TestingEventReceiverFactory(() => received = true, () => { })).For(default(int))
                .Raise(x => new TestingEvent())
                .Finalize(x => x)
                .Project(x => x)
                .Sink();
            
            Assert.True(received);
        }
    }
}
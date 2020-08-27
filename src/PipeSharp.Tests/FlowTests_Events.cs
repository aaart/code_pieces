using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public class FlowTests_Events
    {
        [Fact]
        public void Flow_WhenTestingEventPublished_ExpectEventReceived1()
        {
            bool received = false;

            new StandardBuilder()
                .UseErrorType<TestingFilteringError>()
                .EnableEventSubscription(new TestingSubscription(() => received = true, () => { }))
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
                .UseErrorType<TestingFilteringError>()
                .EnableEventSubscription(new TestingSubscription(() => received = true, () => { }))
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
                .UseErrorType<TestingFilteringError>()
                .EnableEventSubscription(new TestingSubscription(() => received = true, () => { }))
                .For(default(int))
                .Raise(x => new TestingEvent())
                .Finalize(x => x)
                .Project(x => x)
                .Sink();
            
            Assert.True(received);
        }
        
        [Fact]
        public void Flow_WhenTestingEventPublished_ExpectReceiverDisposed1()
        {
            bool disposed = false;

            new StandardBuilder()
                .UseErrorType<TestingFilteringError>()
                .EnableEventSubscription(new TestingSubscription(() => { }, () => disposed = true))
                .For(default(int))
                .Raise(x => new TestingEvent())
                .Finalize(x => { })
                .Sink();

            Assert.True(disposed);
        }

        [Fact]
        public void Flow_WhenTestingEventPublished_ExpectReceiverDisposed2()
        {
            bool disposed = false;

            new StandardBuilder()
                .UseErrorType<TestingFilteringError>()
                .EnableEventSubscription(new TestingSubscription(() => { }, () => disposed = true))
                .For(default(int))
                .Raise(x => new TestingEvent())
                .Finalize(x => x)
                .Sink();
            
            Assert.True(disposed);
        }

        [Fact]
        public void Flow_WhenTestingEventPublished_ExpectReceiverDisposed3()
        {
            bool disposed = false;

            new StandardBuilder()
                .UseErrorType<TestingFilteringError>()
                .EnableEventSubscription(new TestingSubscription(() => { }, () => disposed = true))
                .For(default(int))
                .Raise(x => new TestingEvent())
                .Finalize(x => x)
                .Project(x => x)
                .Sink();
            
            Assert.True(disposed);
        }
    }
}
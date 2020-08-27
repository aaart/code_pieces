using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public class FlowTests_OnChanged
    {
        [Fact]
        public void GivenFlow_WhenOnlyFinalizeDefined_ExpectSingleOnDone()
        {
            int onDoneCount = 0;
            new StandardBuilder()
                .UseErrorType<TestingFilteringError>()
                .OnChanged(() => onDoneCount++)
                .EnableEventSubscription(new TestingSubscription(() => onDoneCount++, () => { }))
                .OnChanged(() => onDoneCount++)
                .For(default(int))
                .Finalize(x => { })
                .Sink();
            Assert.Equal(2, onDoneCount);
        }

        [Fact]
        public void GivenFlow_WhenApplyAndFinalizeDefined_ExpectSingleOnDone()
        {
            int onDoneCount = 0;
            new StandardBuilder()
                .UseErrorType<TestingFilteringError>()
                .OnChanged(() => onDoneCount++)
                .EnableEventSubscription(new TestingSubscription(() => onDoneCount++, () => { }))
                .OnChanged(() => onDoneCount++)
                .For(default(int))
                .Apply(x => x)
                .Apply(x => x)
                .Apply(x => x)
                .Apply(x => x)
                .Apply(x => x)
                .Apply(x => x)
                .Apply(x => x)
                .Apply(x => x)
                .Apply(x => x)
                .Finalize(x => { })
                .Sink();
            Assert.Equal(20, onDoneCount);
        }

        [Fact]
        public void GivenFlow_WhenCheckingDefined_ExpectSingleOnDone()
        {
            int onDoneCount = 0;
            new StandardBuilder()
                .UseErrorType<TestingFilteringError>()
                .OnChanged(() => onDoneCount++)
                .EnableEventSubscription(new TestingSubscription(() => onDoneCount++, () => { }))
                .OnChanged(() => onDoneCount++)
                .For(default(int))
                .Check(x => true, () => new TestingFilteringError())
                .Finalize(x => { })
                .Sink();
            Assert.Equal(2, onDoneCount);
        }
    }
}
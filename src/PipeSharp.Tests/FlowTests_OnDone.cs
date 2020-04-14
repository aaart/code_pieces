using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public class FlowTests_OnDone
    {
        [Fact]
        public void GivenFlow_WhenOnlyFinalizeDefined_ExpectSingleOnDone()
        {
            int onDoneCount = 0;
            new StandardBuilder()
                .WithFilteringError<TestingFilteringError>()
                .OnDone(() => onDoneCount++)
                .WithoutEvents()
                .For(default(int))
                .Finalize(x => { })
                .Sink();
            Assert.Equal(1, onDoneCount);
        }

        [Fact]
        public void GivenFlow_WhenApplyAndFinalizeDefined_ExpectSingleOnDone()
        {
            int onDoneCount = 0;
            new StandardBuilder()
                .WithFilteringError<TestingFilteringError>()
                .OnDone(() => onDoneCount++)
                .WithoutEvents()
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
            Assert.Equal(10, onDoneCount);
        }

        [Fact]
        public void GivenFlow_WhenCheckingDefined_ExpectSingleOnDone()
        {
            int onDoneCount = 0;
            new StandardBuilder()
                .WithFilteringError<TestingFilteringError>()
                .OnDone(() => onDoneCount++)
                .WithoutEvents()
                .For(default(int))
                .Check(x => true, () => new TestingFilteringError())
                .Finalize(x => { })
                .Sink();
            Assert.Equal(1, onDoneCount);
        }
    }
}
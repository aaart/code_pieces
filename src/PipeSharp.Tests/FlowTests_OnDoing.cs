using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public class FlowTests_OnDoing
    {
        [Fact]
        public void GivenFlow_WhenOnlyFinalizeDefined_ExpectSingleOnDoing()
        {
            int onDoingCount = 0;
            new StandardBuilder()
                .OnDoing(() => onDoingCount++)
                .WithFilteringError<TestingFilteringError>()
                .OnDoing(() => onDoingCount++)
                .WithoutEvents()
                .For(default(int))
                .Finalize(x => {})
                .Sink();
            Assert.Equal(2, onDoingCount);
        }
        
        [Fact]
        public void GivenFlow_WhenApplyAndFinalizeDefined_ExpectSingleOnDoing()
        {
            int onDoingCount = 0;
            new StandardBuilder()
                .OnDoing(() => onDoingCount++)
                .WithFilteringError<TestingFilteringError>()
                .OnDoing(() => onDoingCount++)
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
                .Finalize(x => {})
                .Sink();
            Assert.Equal(20, onDoingCount);
        }

        [Fact]
        public void GivenFlow_WhenCheckingDefined_ExpectSingleOnDoing()
        {
            int onDoingCount = 0;
            new StandardBuilder()
                .OnDoing(() => onDoingCount++)
                .WithFilteringError<TestingFilteringError>()
                .OnDoing(() => onDoingCount++)
                .WithoutEvents()
                .For(default(int))
                .Check(x => true, () => new TestingFilteringError())
                .Finalize(x => { })
                .Sink();
            Assert.Equal(2, onDoingCount);
        }
    }
}
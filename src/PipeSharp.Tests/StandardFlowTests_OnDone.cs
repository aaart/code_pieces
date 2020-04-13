using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public partial class StandardFlowTests
    {
        [Fact]
        public void GivenFlow_WhenOnlyFinalizeDefined_ExpectSingleOnDone()
        {
            int onDoneCount = 0;
            _factory
                .For(default(int), () => { }, () => { onDoneCount++; })
                .Finalize(x => { })
                .Sink();
            Assert.Equal(1, onDoneCount);
        }

        [Fact]
        public void GivenFlow_WhenApplyAndFinalizeDefined_ExpectSingleOnDone()
        {
            int onDoneCount = 0;
            _factory
                .For(default(int), () => { }, () => { onDoneCount++; })
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
            _factory
                .For(default(int), () => { }, () => { onDoneCount++; })
                .Check(x => true, () => new TestingFilteringError())
                .Finalize(x => { })
                .Sink();
            Assert.Equal(1, onDoneCount);
        }
    }
}
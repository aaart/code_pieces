using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public partial class StandardFlowTests
    {
        [Fact]
        public void GivenFlow_WhenOnlyFinalizeDefined_ExpectSingleOnDoing()
        {
            int onDoingCount = 0;
            _factory
                .For(default(int), () => { onDoingCount++; }, () => { })
                .Finalize(x => {})
                .Sink();
            Assert.Equal(1, onDoingCount);
        }
        
        [Fact]
        public void GivenFlow_WhenApplyAndFinalizeDefined_ExpectSingleOnDoing()
        {
            int onDoingCount = 0;
            _factory
                .For(default(int), () => { onDoingCount++; }, () => { })
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
            Assert.Equal(10, onDoingCount);
        }

        [Fact]
        public void GivenFlow_WhenCheckingDefined_ExpectSingleOnDoing()
        {
            int onDoingCount = 0;
            _factory
                .For(default(int), () => { onDoingCount++; }, () => { })
                .Check(x => true, () => new TestingFilteringError())
                .Finalize(x => { })
                .Sink();
            Assert.Equal(1, onDoingCount);
        }
    }
}
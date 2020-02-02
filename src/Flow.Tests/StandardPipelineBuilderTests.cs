using Microsoft.Extensions.Logging.Abstractions;
using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public class StandardPipelineBuilderTests
    {
        private readonly StandardFlowFactory<TestingFilteringError> _factory;

        public StandardPipelineBuilderTests()
        {
            _factory = new StandardFlowFactory<TestingFilteringError>(NullLoggerFactory.Instance);
        }

        [Fact]
        public void NewPipelineBuilder_ExpectPipelineReturned()
        {
            Assert.NotNull(_factory.For(new object()));
        }
    }
}

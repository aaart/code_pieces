using System;
using Flow.Tests.TestUtilities;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Flow.Tests
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

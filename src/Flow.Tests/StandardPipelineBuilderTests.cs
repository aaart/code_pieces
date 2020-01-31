using System;
using Flow.Tests.TestUtilities;
using Xunit;

namespace Flow.Tests
{
    public class StandardPipelineBuilderTests
    {
        private readonly StandardFlowFactory<TestingFilteringError> _factory;

        public StandardPipelineBuilderTests()
        {
            _factory = new StandardFlowFactory<TestingFilteringError>();
        }

        [Fact]
        public void NewPipelineBuilder_ExpectPipelineReturned()
        {
            Assert.NotNull(_factory.For(new object()));
        }
    }
}

using System;
using Flow.Tests.TestUtilities;
using Xunit;

namespace Flow.Tests
{
    public class StandardPipelineBuilderTests
    {
        private readonly StandardFlowBuilder<TestingFilteringError> _builder;

        public StandardPipelineBuilderTests()
        {
            _builder = new StandardFlowBuilder<TestingFilteringError>();
        }

        [Fact]
        public void NewPipelineBuilder_ExpectPipelineReturned()
        {
            Assert.NotNull(_builder.For(new object()));
        }
    }
}

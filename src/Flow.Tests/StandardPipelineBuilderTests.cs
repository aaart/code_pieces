using System;
using Xunit;

namespace Flow.Tests
{
    public class StandardPipelineBuilderTests
    {
        private readonly StandardFlowBuilder _builder;

        public StandardPipelineBuilderTests()
        {
            _builder = new StandardFlowBuilder();
        }

        [Fact]
        public void NewPipelineBuilder_ExpectPipelineReturned()
        {
            Assert.NotNull(_builder.For(new object()));
        }
    }
}

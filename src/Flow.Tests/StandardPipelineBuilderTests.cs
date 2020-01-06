using System;
using Xunit;

namespace Flow.Tests
{
    public class StandardPipelineBuilderTests
    {
        private readonly StandardPipelineBuilder _builder;

        public StandardPipelineBuilderTests()
        {
            _builder = new StandardPipelineBuilder();
        }

        [Fact]
        public void NewPipelineBuilder_ExpectPipelineReturned()
        {
            Assert.NotNull(_builder.For(new object()));
        }
    }
}

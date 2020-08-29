using Microsoft.Extensions.Logging.Abstractions;
using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public class StandardBuilderTests
    {
        private readonly IFlowBuilder<TestingFilteringError> _builder;

        public StandardBuilderTests()
        {
            _builder = new StandardBuilder()
                .UseErrorType<TestingFilteringError>();
        }

        [Fact]
        public void NewPipelineBuilder_ExpectPipelineReturned()
        {
            Assert.NotNull(_builder.For(new object()));
        }
    }
}

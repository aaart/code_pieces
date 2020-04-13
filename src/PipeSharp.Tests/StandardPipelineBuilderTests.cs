using Microsoft.Extensions.Logging.Abstractions;
using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public class StandardPipelineBuilderTests
    {
        private readonly IFlowPreDefined<TestingFilteringError> _preDefined;

        public StandardPipelineBuilderTests()
        {
            _preDefined = new StandardBuilder()
                .WithFilteringError<TestingFilteringError>()
                .WithoutEvents();
        }

        [Fact]
        public void NewPipelineBuilder_ExpectPipelineReturned()
        {
            Assert.NotNull(_preDefined.For(new object()));
        }
    }
}

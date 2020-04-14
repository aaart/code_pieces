using Microsoft.Extensions.Logging.Abstractions;
using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public class StandardBuilderTests
    {
        private readonly IFlowPreDefined<TestingFilteringError> _preDefined;

        public StandardBuilderTests()
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

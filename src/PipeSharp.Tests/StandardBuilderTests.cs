using Microsoft.Extensions.Logging.Abstractions;
using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public class StandardBuilderTests
    {
        private readonly IFlowBuilder<TestError> _predefinedFlow = Predefined.Flow;
        
        [Fact]
        public void NewPipelineBuilder_ExpectPipelineReturned()
        {
            Assert.NotNull(_predefinedFlow.For(new object()));
        }
    }
}

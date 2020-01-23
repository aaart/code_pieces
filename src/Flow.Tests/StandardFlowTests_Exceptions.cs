using System;
using Flow.Tests.TestUtilities;
using Xunit;

namespace Flow.Tests
{
    public partial class StandardFlowTests
    {
        [Fact]
        public void StandardFlow_WhenFinalizeThrowsException_ExceptionReturned()
        {
            var result = _builder
                .For(default(int))
                .Finalize(x =>
                {
                    throw new Exception();
                    return x;
                })
                .Sink();

            Assert.NotNull(result.Exception);
        }
        
        [Fact]
        public void StandardFlow_WhenValidateThrowsException_ExceptionReturned()
        {
            var result = _builder
                .For(default(int))
                .Validate(x =>
                {
                    throw new Exception();
                    return true;
                }, () => new TestingFilteringError())
                .Finalize(x => x)
                .Sink();

            Assert.NotNull(result.Exception);
        }
        
        [Fact]
        public void StandardFlow_WhenVerifyThrowsException_ExceptionReturned()
        {
            var result = _builder
                .For(default(int))
                .Apply(x => x)
                .Verify(x =>
                {
                    throw new Exception();
                    return true;
                }, () => new TestingFilteringError())
                .Finalize(x => x)
                .Sink();

            Assert.NotNull(result.Exception);
        }
    }
}
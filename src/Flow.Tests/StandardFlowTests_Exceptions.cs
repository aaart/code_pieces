using System;
using Flow.Tests.TestUtilities;
using Xunit;

namespace Flow.Tests
{
    public partial class StandardFlowTests
    {
        [Fact]
        public void StandardFlow_WhenFinalize1ThrowsException_ExceptionReturned()
        {
            var (_, exception, _) = _builder
                .For(default(int))
                .Finalize(x =>
                {
                    throw new Exception();
                    return x;
                })
                .Sink();

            Assert.NotNull(exception);
        }
        
        [Fact]
        public void StandardFlow_WhenFinalize2ThrowsException_ExceptionReturned()
        {
            var (_, exception, _) = _builder
                .For(default(int))
                .Finalize(x => throw new Exception())
                .Sink();

            Assert.NotNull(exception);
        }
        
        [Fact]
        public void StandardFlow_WhenProjectThrowsException_ExceptionReturned()
        {
            var (_, exception, _) = _builder
                .For(default(int))
                .Finalize(x => x)
                .Project(x =>
                {
                    throw new Exception();
                    return x;
                })
                .Sink();

            Assert.NotNull(exception);
        }
        
        [Fact]
        public void StandardFlow_WhenValidateThrowsException_ExceptionReturned()
        {
            var (_, exception, _) = _builder
                .For(default(int))
                .Validate(x =>
                {
                    throw new Exception();
                    return true;
                }, () => new TestingFilteringError())
                .Finalize(x => x)
                .Sink();

            Assert.NotNull(exception);
        }
        
        [Fact]
        public void StandardFlow_WhenVerifyThrowsException_ExceptionReturned()
        {
            var (_, exception, _) = _builder
                .For(default(int))
                .Apply(x => x)
                .Verify(x =>
                {
                    throw new Exception();
                    return true;
                }, () => new TestingFilteringError())
                .Finalize(x => x)
                .Sink();

            Assert.NotNull(exception);
        }
    }
}
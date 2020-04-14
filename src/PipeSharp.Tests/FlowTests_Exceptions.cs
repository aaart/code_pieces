using System;
using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public class FlowTests_Exceptions
    {
        private readonly IFlowBuilder<TestingFilteringError> _builder;

        public FlowTests_Exceptions()
        {
            _builder = new StandardBuilder()
                .WithFilteringError<TestingFilteringError>();
        }

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
                .Check(x =>
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
                .Check(x =>
                {
                    throw new Exception();
                    return true;
                }, () => new TestingFilteringError())
                .Finalize(x => x)
                .Sink();

            Assert.NotNull(exception);
        }
        
        [Fact]
        public void StandardFlow_WhenVerifyThrowsException_DefaultResultReturned()
        {
            var (summary, _, _) = _builder
                .For(default(int))
                .Apply(x => x)
                .Check(x =>
                {
                    throw new Exception();
                    return true;
                }, () => new TestingFilteringError())
                .Finalize(x => true)
                .Sink();

            Assert.False(summary.Value);
        }
    }
}
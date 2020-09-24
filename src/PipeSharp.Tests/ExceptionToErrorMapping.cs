using System;
using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public class ExceptionToErrorMapping
    {
        private readonly IFlowBuilder<TestError> _predefinedFlow = Predefined.Flow;

        [Fact]
        public void StandardFloe_WhenNullErrorMapperProvided_ExpectException()
        {
            // predefined flow not used intentionally
            Assert.Throws<ArgumentNullException>(() => new StandardBuilder().UseErrorType<TestError>(null));

        }
        
        [Fact]
        public void StandardFlow_WhenExceptionThrownAndNoErrorRaisedAndDeconstructedTo2_ExpectSingleError()
        {
            var (_, errors) = _predefinedFlow
                .For(0)
                .Finalize(x =>
                {
                    throw new Exception();
#pragma warning disable 162
                    return x;
#pragma warning restore 162
                })
                .Sink();
            Assert.Single(errors);
        }

        [Fact]
        public void StandardFlow_WhenExceptionNotThrownAndNoErrors_ExpectValidResult()
        {
            var summary = _predefinedFlow
                .For(0)
                .Finalize(x => x)
                .Sink();
            Assert.Empty(summary.Errors);
        }
    }
}
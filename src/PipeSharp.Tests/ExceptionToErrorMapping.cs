using System;
using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public class ExceptionToErrorMapping
    {
        private readonly IFlowBuilder<TestingFilteringError> _builder;

        public ExceptionToErrorMapping()
        {
            _builder = new StandardBuilder()
                .UseErrorType<TestingFilteringError>();
        }

        [Fact]
        public void StandardFlow_WhenExceptionThrownAndNoErrorRaised_ExpectSingleError()
        {
            var (_, errors) = _builder
                .MapExceptionToErrorOnDeconstruct(ex => new TestingFilteringError {Message = ex.Message, Code = ex.HResult})
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
    }
}
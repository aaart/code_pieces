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
        public void StandardFlow_WhenExceptionThrownAndNoErrorRaisedAndDeconstructedTo2_ExpectSingleError()
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
        
        [Fact]
        public void StandardFlow_WhenExceptionThrownAndNoErrorRaisedAndDeconstructedTo3_ExpectException()
        {
            var (_, _, exception, _) = _builder
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
            Assert.NotNull(exception);
        }
        
        [Fact]
        public void StandardFlow_WhenExceptionThrownAndNoErrorRaisedAndDeconstructedTo3_ExpectNoErrors()
        {
            var (_, _, _, errors) = _builder
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
            Assert.Empty(errors);
        }

        [Fact]
        public void StandardFlow_WhenExceptionNotThrownAndNoErrors_ExpectValidResult()
        {
            var (failed, _, _) = _builder
                .MapExceptionToErrorOnDeconstruct(ex => new TestingFilteringError {Message = ex.Message, Code = ex.HResult})
                .For(0)
                .Finalize(x => x)
                .Sink();
            Assert.False(failed);
        }
    }
}
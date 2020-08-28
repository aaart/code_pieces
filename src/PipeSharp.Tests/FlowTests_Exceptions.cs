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
                .UseErrorType<TestingFilteringError>();
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

        [Fact]
        public void StandardFlow_WhenExceptionHandlerDefinedAndExceptionThrownInCheckMethod_HandlerExecuted()
        {
            bool exceptionHandled = false;
            var (summary, _, _) = _builder
                .HandleException((ex, logger) => exceptionHandled = true)
                .For(default(int))
                .Apply(x => x)
                .Check(x =>
                {
                    throw new Exception();
                    return true;
                }, () => new TestingFilteringError())
                .Finalize(x => true)
                .Sink();

            Assert.True(exceptionHandled);
        }

        [Fact]
        public void StandardFlow_WhenExceptionHandlerDefinedAndExceptionThrownInApplyMethod_HandlerExecuted()
        {
            bool exceptionHandled = false;
            var (summary, _, _) = _builder
                .HandleException((ex, logger) => exceptionHandled = true)
                .For(default(int))
                .Apply(x =>
                {
                    throw new Exception();
                    return x;
                })
                .Check(x => true, () => new TestingFilteringError())
                .Finalize(x => true)
                .Sink();

            Assert.True(exceptionHandled);
        }

        [Fact]
        public void StandardFlow_WhenExceptionHandlerDefinedAndExceptionThrownInFinalizeMethod_HandlerExecuted()
        {
            bool exceptionHandled = false;
            var (summary, _, _) = _builder
                .HandleException((ex, logger) => exceptionHandled = true)
                .For(default(int))
                .Apply(x => x)
                .Check(x => true, () => new TestingFilteringError())
                .Finalize(x =>
                {
                    throw new Exception();
                    return true;
                })
                .Sink();

            Assert.True(exceptionHandled);
        }

        [Fact]
        public void StandardFlow_WhenExceptionHandlerDefinedAndExceptionThrownInProjectMethod_HandlerExecuted()
        {
            bool exceptionHandled = false;
            var (summary, _, _) = _builder
                .HandleException((ex, logger) => exceptionHandled = true)
                .For(default(int))
                .Apply(x => x)
                .Check(x => true, () => new TestingFilteringError())
                .Finalize(x => true)
                .Project(x =>
                {
                    throw new Exception();
                    return x;
                })
                .Sink();

            Assert.True(exceptionHandled);
        }

        [Fact]
        public void StandardFlow_WhenExceptionHandlerDefinedAndExceptionThrownInRaiseMethod_HandlerExecuted()
        {
            bool exceptionHandled = false;
            var (summary, _, _) = _builder
                .HandleException((ex, logger) => exceptionHandled = true)
                .EnableEventSubscription(new TestingSubscription(() => { }, () => { }))
                .For(default(int))
                .Raise(x =>
                {
                    throw new Exception();
                    return new TestingEvent();
                })
                .Apply(x => x)
                
                .Check(x => true, () => new TestingFilteringError())
                .Finalize(x => true)
                .Project(x => x)
                .Sink();

            Assert.True(exceptionHandled);
        }

        [Fact]
        public void StandardFlow_WhenExceptionHandlerDefinedAndExceptionNotThrown_HandlerExecuted()
        {
            bool exceptionHandled = false;
            var (summary, _, _) = _builder
                .HandleException((ex, logger) => exceptionHandled = true)
                .For(default(int))
                .Apply(x => x)
                .Check(x => true, () => new TestingFilteringError())
                .Finalize(x => true)
                .Project(x => x)
                .Sink();

            Assert.False(exceptionHandled);
        }
    }
}
using System;
using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public class Exceptions
    {
        private readonly IFlowBuilder<TestError> _predefinedFlow = Predefined.Flow;

        [Fact]
        public void StandardFlow_WhenFinalize1ThrowsException_ExceptionReturned()
        {
            var summary = _predefinedFlow
                .For(default(int))
                .Finalize(x =>
                {
                    throw new Exception();
#pragma warning disable 162
                    return x;
#pragma warning restore 162
                })
                .Sink();

            Assert.Single(summary.Errors);
        }

        [Fact]
        public void StandardFlow_WhenFinalize2ThrowsException_ExceptionReturned()
        {
            var summary = _predefinedFlow
                .For(default(int))
                .Finalize(x => throw new Exception())
                .Sink();

            Assert.Single(summary.Errors);
        }

        [Fact]
        public void StandardFlow_WhenProjectThrowsException_ExceptionReturned()
        {
            var summary = _predefinedFlow
                .For(default(int))
                .Finalize(x => x)
                .Project(x =>
                {
                    throw new Exception();
#pragma warning disable 162
                    return x;
#pragma warning restore 162
                })
                .Sink();

            Assert.Single(summary.Errors);
        }

        [Fact]
        public void StandardFlow_WhenValidateThrowsException_ExceptionReturned()
        {
            var summary = _predefinedFlow
                .For(default(int))
                .Check(x =>
                {
                    throw new Exception();
#pragma warning disable 162
                    return true;
#pragma warning restore 162
                }, () => new TestError())
                .Finalize(x => x)
                .Sink();

            Assert.Single(summary.Errors);
        }

        [Fact]
        public void StandardFlow_WhenVerifyThrowsException_ExceptionReturned()
        {
            var summary = _predefinedFlow
                .For(default(int))
                .Apply(x => x)
                .Check(x =>
                {
                    throw new Exception();
#pragma warning disable 162
                    return true;
#pragma warning restore 162
                }, () => new TestError())
                .Finalize(x => x)
                .Sink();

            Assert.Single(summary.Errors);
        }

        [Fact]
        public void StandardFlow_WhenVerifyThrowsException_DefaultResultReturned()
        {
            var summary = _predefinedFlow
                .For(default(int))
                .Apply(x => x)
                .Check(x =>
                {
                    throw new Exception();
#pragma warning disable 162
                    return true;
#pragma warning restore 162
                }, () => new TestError())
                .Finalize(x => true)
                .Sink();

            Assert.False(summary.Value);
        }

        [Fact]
        public void StandardFlow_WhenExceptionHandlerDefinedAndExceptionThrownInCheckMethod_HandlerExecuted()
        {
            bool exceptionHandled = false;
            _predefinedFlow
                .HandleException((ex, logger) => exceptionHandled = true)
                .For(default(int))
                .Apply(x => x)
                .Check(x =>
                {
                    throw new Exception();
#pragma warning disable 162
                    return true;
#pragma warning restore 162
                }, () => new TestError())
                .Finalize(x => true)
                .Sink();

            Assert.True(exceptionHandled);
        }

        [Fact]
        public void StandardFlow_WhenExceptionHandlerDefinedAndExceptionThrownInApplyMethod_HandlerExecuted()
        {
            bool exceptionHandled = false;
            _predefinedFlow
                .HandleException((ex, logger) => exceptionHandled = true)
                .For(default(int))
                .Apply(x =>
                {
                    throw new Exception();
#pragma warning disable 162
                    return x;
#pragma warning restore 162
                })
                .Check(x => true, () => new TestError())
                .Finalize(x => true)
                .Sink();

            Assert.True(exceptionHandled);
        }

        [Fact]
        public void StandardFlow_WhenExceptionHandlerDefinedAndExceptionThrownInFinalizeMethod_HandlerExecuted()
        {
            bool exceptionHandled = false;
            _predefinedFlow
                .HandleException((ex, logger) => exceptionHandled = true)
                .For(default(int))
                .Apply(x => x)
                .Check(x => true, () => new TestError())
                .Finalize(x =>
                {
                    throw new Exception();
#pragma warning disable 162
                    return true;
#pragma warning restore 162
                })
                .Sink();

            Assert.True(exceptionHandled);
        }

        [Fact]
        public void StandardFlow_WhenExceptionHandlerDefinedAndExceptionThrownInProjectMethod_HandlerExecuted()
        {
            bool exceptionHandled = false;
            _predefinedFlow
                .HandleException((ex, logger) => exceptionHandled = true)
                .For(default(int))
                .Apply(x => x)
                .Check(x => true, () => new TestError())
                .Finalize(x => true)
                .Project(x =>
                {
                    throw new Exception();
#pragma warning disable 162
                    return x;
#pragma warning restore 162
                })
                .Sink();

            Assert.True(exceptionHandled);
        }

        [Fact]
        public void StandardFlow_WhenExceptionHandlerDefinedAndExceptionThrownInRaiseMethod_HandlerExecuted()
        {
            bool exceptionHandled = false;
            _predefinedFlow
                .HandleException((ex, logger) => exceptionHandled = true)
                .EnableEventSubscription(new TestingSubscription(() => { }, () => { }))
                .For(default(int))
                .Raise(x =>
                {
                    throw new Exception();
#pragma warning disable 162
                    return new TestingEvent();
#pragma warning restore 162
                })
                .Apply(x => x)
                
                .Check(x => true, () => new TestError())
                .Finalize(x => true)
                .Project(x => x)
                .Sink();

            Assert.True(exceptionHandled);
        }

        [Fact]
        public void StandardFlow_WhenExceptionHandlerDefinedAndExceptionNotThrown_HandlerExecuted()
        {
            bool exceptionHandled = false;
            _predefinedFlow
                .HandleException((ex, logger) => exceptionHandled = true)
                .For(default(int))
                .Apply(x => x)
                .Check(x => true, () => new TestError())
                .Finalize(x => true)
                .Project(x => x)
                .Sink();

            Assert.False(exceptionHandled);
        }
    }
}
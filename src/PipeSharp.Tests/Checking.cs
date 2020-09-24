using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public class Checking
    {

        private readonly IFlowBuilder<TestError> _predeFlow = Predefined.Flow;

        [Fact]
        public void Flow_WhenValidationFails_ExpectSingleErrorInResult()
        {
            var summary = _predeFlow
                .For(new { Msg = "Mockery" })
                .Check(x => false, () => new TestError())
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Single(summary.Errors);
        }

        [Fact]
        public void Flow_WhenValidationFails_ExpectSingleErrorInResult2()
        {
            var summary = _predeFlow
                .For(new { Msg = "Mockery" })
                .Check(x => false, () => new TestError())
                .Finalize(x => false)
                .Sink();
            Assert.Single(summary.Errors);
        }

        [Fact]
        public void Flow_WhenValidationFails_ExpectSingleErrorInResult3()
        {
            var summary = _predeFlow
                .For(new { Msg = "Mockery" })
                .Check(x => false, () => new TestError())
                .Finalize(x => false)
                .Project(x => 1)
                .Sink();
            Assert.Single(summary.Errors);
        }

        [Fact]
        public void Flow_WhenVerificationFails_ExpectSingleErrorInResult()
        {
            var summary = _predeFlow
                .For(new { Msg = "Mockery" })
                .Apply(x => x)
                .Check(x => false, () => new TestError())
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Single(summary.Errors);
        }

        [Fact]
        public void Flow_WhenVerificationFailsTwice_ExpectTwoErrorsInResult()
        {
            var summary = _predeFlow
                .For(new { Msg = "Mockery" })
                .Apply(x => x)
                .Check(x => false, () => new TestError())
                .Check(x => false, () => new TestError())
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Equal(2, summary.Errors.Length);
        }
        
        [Fact]
        public void Flow_WhenValidationFailsTwice_ExpectTwoErrorsInResult()
        {
            var summary = _predeFlow
                .For(new { Msg = "Mockery" })
                .Check(x => false, () => new TestError())
                .Check(x => false, () => new TestError())
                .Apply(x => x)
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Equal(2, summary.Errors.Length);
        }
        
        [Fact]
        public void Flow_WhenValidationFailsTwiceAndVerificationFails_ExpectTwoErrorsInResult()
        {
            var summary = _predeFlow
                .For(new { Msg = "Mockery" })
                .Check(x => false, () => new TestError())
                .Check(x => false, () => new TestError())
                .Apply(x => x)
                .Check(x => false, () => new TestError())
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Equal(2, summary.Errors.Length);
        }

        [Fact]
        public void Flow_WhenValidationFails_PipelineStopped()
        {
            bool executed = false;
            _predeFlow
                .For(new { Msg = "Mockery" })
                .Check(x => false, () => new TestError())
                .Finalize(x =>
                {
                    executed = true;
                })
                .Sink();
            Assert.False(executed);
        }

        [Fact]
        public void Flow_WhenValidationFails_PipelineStopped2()
        {
            bool executed = false;
            _predeFlow
                .For(new { Msg = "Mockery" })
                .Check(x => x, x => false, () => new TestError())
                .Finalize(x =>
                {
                    executed = true;
                    return x;
                })
                .Sink();
            Assert.False(executed);
        }

        [Fact]
        public void Flow_WhenValidationFails_PipelineStopped3()
        {
            bool executed = false;
            _predeFlow
                .For(new { Msg = "Mockery" })
                .Check(x => new TestingInput(), new TestingFilter())
                .Finalize(x =>
                {
                    executed = true;
                    return x;
                })
                .Sink();
            Assert.False(executed);
        }

        [Fact]
        public void Flow_WhenValidationFails_PipelineStopped4()
        {
            bool executed = false;
            _predeFlow
                .For(new TestingInput())
                .Check(new TestingFilter())
                .Finalize(x =>
                {
                    executed = true;
                    return x;
                })
                .Sink();
            Assert.False(executed);
        }

        [Fact]
        public void Flow_WhenValidationChangesTarget_ExpectFinalizeExecuted()
        {
            bool executed = false;
            _predeFlow
                .For(new { Prop = true })
                .Check(x => x.Prop, x => x, () => new TestError())
                .Finalize(x =>
                {
                    executed = true;
                })
                .Sink();
            Assert.True(executed);
        }

        [Fact]
        public void Flow_WhenVerificationFails_PipelineStopped()
        {
            bool executed = false;
            _predeFlow
                .For(new { Msg = "Mockery" })
                .Apply(x => x)
                .Check(x => false, () => new TestError())
                .Finalize(x =>
                {
                    executed = true;
                    return x;
                })
                .Sink();
            Assert.False(executed);
        }

        [Fact]
        public void Flow_WhenVerificationFails_PipelineStopped2()
        {
            bool executed = false;
            _predeFlow
                .For(new { Msg = "Mockery" })
                .Apply(x => x)
                .Check(x => x.Msg, x => false, () => new TestError())
                .Finalize(x =>
                {
                    executed = true;
                    return x;
                })
                .Sink();
            Assert.False(executed);
        }

        [Fact]
        public void Flow_WhenVerificationFails_PipelineStopped3()
        {
            bool executed = false;
            _predeFlow
                .For(new { Msg = "Mockery" })
                .Apply(x => x)
                .Check(x => new TestingInput(), new TestingFilter())
                .Finalize(x =>
                {
                    executed = true;
                    return x;
                })
                .Sink();
            Assert.False(executed);
        }

        [Fact]
        public void Flow_WhenVerificationFails_PipelineStopped4()
        {
            bool executed = false;
            _predeFlow
                .For(new TestingInput())
                .Apply(x => x)
                .Check(new TestingFilter())
                .Finalize(x =>
                {
                    executed = true;
                    return x;
                })
                .Sink();
            Assert.False(executed);
        }

        [Fact]
        public void Flow_WhenCheckingFails_ExpectNullException1()
        {
            var summary = _predeFlow
                .For(new TestingInput())
                .Check(x => false, () => new TestError())
                .Finalize(x => x)
                .Sink();
            Assert.Single(summary.Errors);
        }

        [Fact]
        public void Flow_WhenCheckingFails_ExpectNullException2()
        {
            var summary = _predeFlow
                .For(new TestingInput())
                .Check(x => false, () => new TestError())
                .Finalize(x => { })
                .Sink();
            Assert.Single(summary.Errors);
        }
    }
}
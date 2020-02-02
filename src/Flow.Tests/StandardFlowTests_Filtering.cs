using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public partial class StandardFlowTests
    {
        [Fact]
        public void Flow_WhenValidationFails_ExpectSingleErrorInResult()
        {
            var (_, _, filteringErrors) = _factory
                .For(new { Msg = "Mockery" })
                .Validate(x => false, () => new TestingFilteringError())
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Single(filteringErrors);
        }

        [Fact]
        public void Flow_WhenValidationFails_ExpectSingleErrorInResult2()
        {
            var (_, _, filteringErrors) = _factory
                .For(new { Msg = "Mockery" })
                .Validate(x => false, () => new TestingFilteringError())
                .Finalize(x => false)
                .Sink();
            Assert.Single(filteringErrors);
        }

        [Fact]
        public void Flow_WhenValidationFails_ExpectSingleErrorInResult3()
        {
            var (_, _, filteringErrors) = _factory
                .For(new { Msg = "Mockery" })
                .Validate(x => false, () => new TestingFilteringError())
                .Finalize(x => false)
                .Project(x => 1)
                .Sink();
            Assert.Single(filteringErrors);
        }

        [Fact]
        public void Flow_WhenVerificationFails_ExpectSingleErrorInResult()
        {
            var (_, _, filteringErrors) = _factory
                .For(new { Msg = "Mockery" })
                .Apply(x => x)
                .Verify(x => false, () => new TestingFilteringError())
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Single(filteringErrors);
        }

        [Fact]
        public void Flow_WhenVerificationFailsTwice_ExpectTwoErrorsinResult()
        {
            var (_, _, filteringErrors) = _factory
                .For(new { Msg = "Mockery" })
                .Apply(x => x)
                .Verify(x => false, () => new TestingFilteringError())
                .Verify(x => false, () => new TestingFilteringError())
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Equal(2, filteringErrors.Length);
        }
        
        [Fact]
        public void Flow_WhenValidationFailsTwice_ExpectTwoErrorsinResult()
        {
            var (_, _, filteringErrors) = _factory
                .For(new { Msg = "Mockery" })
                .Validate(x => false, () => new TestingFilteringError())
                .Validate(x => false, () => new TestingFilteringError())
                .Apply(x => x)
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Equal(2, filteringErrors.Length);
        }
        
        [Fact]
        public void Flow_WhenValidationFailsTwiceAndVerificationFails_ExpectTwoErrorsinResult()
        {
            var (_, _, filteringErrors) = _factory
                .For(new { Msg = "Mockery" })
                .Validate(x => false, () => new TestingFilteringError())
                .Validate(x => false, () => new TestingFilteringError())
                .Apply(x => x)
                .Verify(x => false, () => new TestingFilteringError())
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Equal(2, filteringErrors.Length);
        }

        [Fact]
        public void Flow_WhenValidationFails_PipelineStopped()
        {
            bool executed = false;
            _factory
                .For(new { Msg = "Mockery" })
                .Validate(x => false, () => new TestingFilteringError())
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
            _factory
                .For(new { Msg = "Mockery" })
                .Validate(x => x, x => false, () => new TestingFilteringError())
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
            _factory
                .For(new { Msg = "Mockery" })
                .Validate(x => new TestingInput(), new TestingFilter())
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
            _factory
                .For(new TestingInput())
                .Validate(new TestingFilter())
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
            _factory
                .For(new { Prop = true })
                .Validate(x => x.Prop, x => x, () => new TestingFilteringError())
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
            _factory
                .For(new { Msg = "Mockery" })
                .Apply(x => x)
                .Verify(x => false, () => new TestingFilteringError())
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
            _factory
                .For(new { Msg = "Mockery" })
                .Apply(x => x)
                .Verify(x => x.Msg, x => false, () => new TestingFilteringError())
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
            _factory
                .For(new { Msg = "Mockery" })
                .Apply(x => x)
                .Verify(x => new TestingInput(), new TestingFilter())
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
            _factory
                .For(new TestingInput())
                .Apply(x => x)
                .Verify(new TestingFilter())
                .Finalize(x =>
                {
                    executed = true;
                    return x;
                })
                .Sink();
            Assert.False(executed);
        }
    }
}
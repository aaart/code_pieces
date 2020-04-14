using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public partial class FlowTests_Filtering
    {

        private readonly IFlowPreDefined<TestingFilteringError> _preDefined;

        public FlowTests_Filtering()
        {
            _preDefined = new StandardBuilder()
                .WithFilteringError<TestingFilteringError>()
                .WithoutEvents();
        }

        [Fact]
        public void Flow_WhenValidationFails_ExpectSingleErrorInResult()
        {
            var (_, _, filteringErrors) = _preDefined
                .For(new { Msg = "Mockery" })
                .Check(x => false, () => new TestingFilteringError())
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Single(filteringErrors);
        }

        [Fact]
        public void Flow_WhenValidationFails_ExpectSingleErrorInResult2()
        {
            var (_, _, filteringErrors) = _preDefined
                .For(new { Msg = "Mockery" })
                .Check(x => false, () => new TestingFilteringError())
                .Finalize(x => false)
                .Sink();
            Assert.Single(filteringErrors);
        }

        [Fact]
        public void Flow_WhenValidationFails_ExpectSingleErrorInResult3()
        {
            var (_, _, filteringErrors) = _preDefined
                .For(new { Msg = "Mockery" })
                .Check(x => false, () => new TestingFilteringError())
                .Finalize(x => false)
                .Project(x => 1)
                .Sink();
            Assert.Single(filteringErrors);
        }

        [Fact]
        public void Flow_WhenVerificationFails_ExpectSingleErrorInResult()
        {
            var (_, _, filteringErrors) = _preDefined
                .For(new { Msg = "Mockery" })
                .Apply(x => x)
                .Check(x => false, () => new TestingFilteringError())
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Single(filteringErrors);
        }

        [Fact]
        public void Flow_WhenVerificationFailsTwice_ExpectTwoErrorsinResult()
        {
            var (_, _, filteringErrors) = _preDefined
                .For(new { Msg = "Mockery" })
                .Apply(x => x)
                .Check(x => false, () => new TestingFilteringError())
                .Check(x => false, () => new TestingFilteringError())
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Equal(2, filteringErrors.Length);
        }
        
        [Fact]
        public void Flow_WhenValidationFailsTwice_ExpectTwoErrorsinResult()
        {
            var (_, _, filteringErrors) = _preDefined
                .For(new { Msg = "Mockery" })
                .Check(x => false, () => new TestingFilteringError())
                .Check(x => false, () => new TestingFilteringError())
                .Apply(x => x)
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Equal(2, filteringErrors.Length);
        }
        
        [Fact]
        public void Flow_WhenValidationFailsTwiceAndVerificationFails_ExpectTwoErrorsinResult()
        {
            var (_, _, filteringErrors) = _preDefined
                .For(new { Msg = "Mockery" })
                .Check(x => false, () => new TestingFilteringError())
                .Check(x => false, () => new TestingFilteringError())
                .Apply(x => x)
                .Check(x => false, () => new TestingFilteringError())
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Equal(2, filteringErrors.Length);
        }

        [Fact]
        public void Flow_WhenValidationFails_PipelineStopped()
        {
            bool executed = false;
            _preDefined
                .For(new { Msg = "Mockery" })
                .Check(x => false, () => new TestingFilteringError())
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
            _preDefined
                .For(new { Msg = "Mockery" })
                .Check(x => x, x => false, () => new TestingFilteringError())
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
            _preDefined
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
            _preDefined
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
            _preDefined
                .For(new { Prop = true })
                .Check(x => x.Prop, x => x, () => new TestingFilteringError())
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
            _preDefined
                .For(new { Msg = "Mockery" })
                .Apply(x => x)
                .Check(x => false, () => new TestingFilteringError())
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
            _preDefined
                .For(new { Msg = "Mockery" })
                .Apply(x => x)
                .Check(x => x.Msg, x => false, () => new TestingFilteringError())
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
            _preDefined
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
            _preDefined
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
        public void Flow_WhenCheckingFails_ExpectFailedResult1()
        {
            var (res, _, _) = _preDefined
                .For(new TestingInput())
                .Check(x => false, () => new TestingFilteringError())
                .Finalize(x => x)
                .Sink();
            Assert.True(res.Failed);
        }
        
        [Fact]
        public void Flow_WhenCheckingFails_ExpectFailedResult2()
        {
            var (res, _, _) = _preDefined
                .For(new TestingInput())
                .Check(x => false, () => new TestingFilteringError())
                .Finalize(x => { })
                .Sink();
            Assert.True(res.Failed);
        }

        [Fact]
        public void Flow_WhenCheckingFails_ExpectNullException1()
        {
            var (_, ex, _) = _preDefined
                .For(new TestingInput())
                .Check(x => false, () => new TestingFilteringError())
                .Finalize(x => x)
                .Sink();
            Assert.Null(ex);
        }

        [Fact]
        public void Flow_WhenCheckingFails_ExpectNullException2()
        {
            var (_, ex, _) = _preDefined
                .For(new TestingInput())
                .Check(x => false, () => new TestingFilteringError())
                .Finalize(x => { })
                .Sink();
            Assert.Null(ex);
        }
    }
}
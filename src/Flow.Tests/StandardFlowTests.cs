using Flow.Tests.TestUtilities;
using Xunit;

namespace Flow.Tests
{
    public class StandardFlowTests
    {
        private readonly StandardPipelineBuilder _builder;

        public StandardFlowTests()
        {
            _builder = new StandardPipelineBuilder();
        }

        [Fact]
        public void MockeryInput_WhenNoFiltersAndModificationsApplied_ExpectSuccessWithNoValueResult()
        {
            var result = _builder
                .For(new { Msg = "Mockery" })
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.False(result.Failed);
        }

        [Fact]
        public void DefultInt_WhenNoFiltersAndModificationsApplied_ExpectSuccessWithIntDefaultResult()
        {
            var input = default(int);
            var result = _builder
                .For(input)
                .Finalize(x => x)
                .Sink();
            Assert.Equal(input, result.Result);
        }
        
        [Fact]
        public void One_WhenNoFiltersAndModificationsApplied_ExpectSuccessWithNumberOneResult()
        {
            var input = 1;
            var result = _builder
                .For(input)
                .Finalize(x => x)
                .Sink();
            Assert.Equal(input, result.Result);
        }

        [Fact]
        public void DefaultIntAppliedOne_WhenNoFiltersAppliedButModificationsApplied_ExpectSuccessWithNumberOnResult()
        {
            var applied = 1;
            var result = _builder
                .For(0)
                .Apply(x => applied)
                .Finalize(x => x)
                .Sink();
            Assert.Equal(applied, result.Result);
        }
        
        [Fact]
        public void DefaultIntAppliedOneAndTwo_WhenNoFiltersAppliedButModificationsApplied_ExpectSuccessWithNumberOnResult()
        {
            var applied = 2;
            var result = _builder
                .For(0)
                .Apply(x => 1)
                .Apply(x => applied)
                .Finalize(x => x)
                .Sink();
            Assert.Equal(applied, result.Result);
        }

        [Fact]
        public void DefaultIn_WhenApplicationChangesType_ExpectSuccessWithEmptyString()
        {
            var applied = string.Empty;
            var result = _builder
                .For(0)
                .Apply(x => 1)
                .Apply(x => applied)
                .Finalize(x => x)
                .Sink();
            Assert.Equal(applied, result.Result);
        }
        
        [Fact]
        public void DefaultIn_WhenApplicationChangesType2_ExpectSuccessWithEmptyString()
        {
            var result = _builder
                .For(0)
                .Apply(x => x + 2)
                .Apply(x => x.ToString())
                .Apply(x => x.ToString() + "1")
                .Finalize(x => x)
                .Sink();
            Assert.Equal("21", result.Result);
        }

        [Fact]
        public void Flow_WhenValidationFails_ExpectSingleErrorInResult()
        {
            var result = _builder
                .For(new { Msg = "Mockery" })
                .Validate(x => false, () => new TestingError())
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Equal(1, result.Errors.Count);
        }
        
        [Fact]
        public void Flow_WhenVerificationFails_ExpectSingleErrorInResult()
        {
            var result = _builder
                .For(new { Msg = "Mockery" })
                .Apply(x => x)
                .Verify(x => false, () => new TestingError())
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Equal(1, result.Errors.Count);
        }


        [Fact]
        public void Flow_WhenValidationFails_FinalizeNotExecuted()
        {
            bool executed = false;
            var result = _builder
                .For(new { Msg = "Mockery" })
                .Validate(x => false, () => new TestingError())
                .Finalize(x =>
                {
                    executed = true;
                })
                .Sink();
            Assert.False(executed);
        }
        
        [Fact]
        public void Flow_WhenValidationFails_FinalizeNotExecuted2()
        {
            bool executed = false;
            var result = _builder
                .For(new { Msg = "Mockery" })
                .Validate(x => false, () => new TestingError())
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
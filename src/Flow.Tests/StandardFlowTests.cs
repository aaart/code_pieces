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

        //[Fact]
        //public void Flow_WhenValidationFails_ExpectSingleErrorInResult()
        //{
        //    var result = _builder
        //        .For(new { Msg = "Mockery" })
        //        .Validate(x => false, () => new TestingError())
        //        .Finalize(Predefined.EmptyMethod)
        //        .Sink();
        //    Assert.Equal(1, result.Errors.Count);
        //}
    }
}
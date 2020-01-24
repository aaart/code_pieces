using Flow.Tests.TestUtilities;
using Xunit;

namespace Flow.Tests
{
    public partial class StandardFlowTests
    {
        private readonly StandardPipelineBuilder _builder;

        public StandardFlowTests()
        {
            _builder = new StandardPipelineBuilder();
        }

        [Fact]
        public void InputDoesMatter_WhenNoErrorsOccur_ExpectValidExecutionCount()
        {
            int count = 0;
            var finalized = _builder
                .For(new { })
                .Validate(x =>
                {
                    count++;
                    return true;
                }, () => new TestingFilteringError())
                .Apply(x =>
                {
                    count++;
                    return x;
                })
                .Verify(x =>
                {
                    count++;
                    return true;
                }, () => new TestingFilteringError())
                .Verify(x =>
                {
                    count++;
                    return new { prop = x };
                }, x =>
                {
                    count++;
                    return true;
                }, () => new TestingFilteringError())
                .Apply(x =>
                {
                    count++;
                    return x;
                })
                .Finalize(x =>
                {
                    count++;
                })
                .Sink();
            Assert.Equal(7, count);
        }

        [Fact]
        public void InputDoesMatter_WhenNoErrorsOccur_ExpectValidExecutionCount2()
        {
            int count = 0;
            var (result, _, _) = _builder
                .For(new { })
                .Validate(x =>
                {
                    count++;
                    return true;
                }, () => new TestingFilteringError())
                .Apply(x =>
                {
                    count++;
                    return x;
                })
                .Verify(x =>
                {
                    count++;
                    return true;
                }, () => new TestingFilteringError())
                .Verify(x =>
                {
                    count++;
                    return new { prop = x };
                }, x =>
                {
                    count++;
                    return true;
                }, () => new TestingFilteringError())
                .Apply(x =>
                {
                    count++;
                    return x;
                })
                .Finalize(x => count + 1)
                .Project(x => x + 1)
                .Sink();
            Assert.Equal(8, result.Value);
        }

        [Fact]
        public void MockeryInput_WhenNoFiltersAndModificationsApplied_ExpectSuccessWithNoValueResult()
        {
            var (result, exception, readOnlyCollection) = _builder
                .For(new { Msg = "Mockery" })
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.False(result.Failed);
        }

        [Fact]
        public void DefultInt_WhenNoFiltersAndModificationsApplied_ExpectSuccessWithIntDefaultResult()
        {
            var input = default(int);
            var (result, _, _) = _builder
                .For(input)
                .Finalize(x => x)
                .Sink();
            Assert.Equal(input, result.Value);
        }

        [Fact]
        public void One_WhenNoFiltersAndModificationsApplied_ExpectSuccessWithNumberOneResult()
        {
            var input = 1;
            var (result, _, _) = _builder
                .For(input)
                .Finalize(x => x)
                .Sink();
            Assert.Equal(input, result.Value);
        }

        [Fact]
        public void DefaultIntAppliedOne_WhenNoFiltersAppliedButModificationsApplied_ExpectSuccessWithNumberOnResult()
        {
            var applied = 1;
            var (result, _, _) = _builder
                .For(0)
                .Apply(x => applied)
                .Finalize(x => x)
                .Sink();
            Assert.Equal(applied, result.Value);
        }

        [Fact]
        public void DefaultIntAppliedOneAndTwo_WhenNoFiltersAppliedButModificationsApplied_ExpectSuccessWithNumberOnResult()
        {
            var applied = 2;
            var (result, _, _) = _builder
                .For(0)
                .Apply(x => 1)
                .Apply(x => applied)
                .Finalize(x => x)
                .Sink();
            Assert.Equal(applied, result.Value);
        }

        [Fact]
        public void DefaultIn_WhenApplicationChangesType_ExpectSuccessWithEmptyString()
        {
            var applied = string.Empty;
            var (result, _, _) = _builder
                .For(0)
                .Apply(x => 1)
                .Apply(x => applied)
                .Finalize(x => x)
                .Sink();
            Assert.Equal(applied, result.Value);
        }

        [Fact]
        public void DefaultIn_WhenApplicationChangesType2_ExpectSuccessWithEmptyString()
        {
            var (result, _, _) = _builder
                .For(0)
                .Apply(x => x + 2)
                .Apply(x => x.ToString())
                .Apply(x => x.ToString() + "1")
                .Finalize(x => x)
                .Sink();
            Assert.Equal("21", result.Value);
        }

        [Fact]
        public void Flow_WhenValidationFails_ExpectSingleErrorInResult()
        {
            var (_, _, filteringErrors) = _builder
                .For(new { Msg = "Mockery" })
                .Validate(x => false, () => new TestingFilteringError())
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Equal(1, filteringErrors.Count);
        }

        [Fact]
        public void Flow_WhenValidationFails_ExpectSingleErrorInResult2()
        {
            var (_, _, filteringErrors) = _builder
                .For(new { Msg = "Mockery" })
                .Validate(x => false, () => new TestingFilteringError())
                .Finalize(x => false)
                .Sink();
            Assert.Equal(1, filteringErrors.Count);
        }

        [Fact]
        public void Flow_WhenValidationFails_ExpectSingleErrorInResult3()
        {
            var (_, _, filteringErrors) = _builder
                .For(new { Msg = "Mockery" })
                .Validate(x => false, () => new TestingFilteringError())
                .Finalize(x => false)
                .Project(x => 1)
                .Sink();
            Assert.Equal(1, filteringErrors.Count);
        }

        [Fact]
        public void Flow_WhenVerificationFails_ExpectSingleErrorInResult()
        {
            var (_, _, filteringErrors) = _builder
                .For(new { Msg = "Mockery" })
                .Apply(x => x)
                .Verify(x => false, () => new TestingFilteringError())
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Equal(1, filteringErrors.Count);
        }


        [Fact]
        public void Flow_WhenValidationFails_PipelineStopped()
        {
            bool executed = false;
            var result = _builder
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
            var result = _builder
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
            var result = _builder
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
            var result = _builder
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
            var result = _builder
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
            var result = _builder
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
            var result = _builder
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
            var result = _builder
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
            var result = _builder
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
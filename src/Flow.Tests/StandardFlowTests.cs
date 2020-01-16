using Flow.Tests.TestUtilities;
using Xunit;

namespace Flow.Tests
{
    public class StandardFlowTests
    {
        public class TestingInput
        {

        }

        public class TestingFilter : IFilter<TestingInput>
        {
            public bool Check(TestingInput target, out IError error)
            {
                error = new TestingError();
                return false;
            }
        }

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
                }, () => new TestingError())
                .Apply(x =>
                {
                    count++;
                    return x;
                })
                .Verify(x =>
                {
                    count++;
                    return true;
                }, () => new TestingError())
                .Verify(x =>
                {
                    count++;
                    return new { prop = x };
                }, x =>
                {
                    count++;
                    return true;
                }, () => new TestingError())
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
            var finalized = _builder
                .For(new { })
                .Validate(x =>
                {
                    count++;
                    return true;
                }, () => new TestingError())
                .Apply(x =>
                {
                    count++;
                    return x;
                })
                .Verify(x =>
                {
                    count++;
                    return true;
                }, () => new TestingError())
                .Verify(x =>
                {
                    count++;
                    return new { prop = x };
                }, x =>
                {
                    count++;
                    return true;
                }, () => new TestingError())
                .Apply(x =>
                {
                    count++;
                    return x;
                })
                .Finalize(x => count + 1)
                .Sink();
            Assert.Equal(7, finalized.Value);
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
            Assert.Equal(input, result.Value);
        }

        [Fact]
        public void One_WhenNoFiltersAndModificationsApplied_ExpectSuccessWithNumberOneResult()
        {
            var input = 1;
            var result = _builder
                .For(input)
                .Finalize(x => x)
                .Sink();
            Assert.Equal(input, result.Value);
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
            Assert.Equal(applied, result.Value);
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
            Assert.Equal(applied, result.Value);
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
            Assert.Equal(applied, result.Value);
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
            Assert.Equal("21", result.Value);
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
        public void Flow_WhenValidationFails_ExpectSingleErrorInResult2()
        {
            var result = _builder
                .For(new { Msg = "Mockery" })
                .Validate(x => false, () => new TestingError())
                .Finalize(x => false)
                .Sink();
            Assert.Equal(1, result.Errors.Count);
        }

        [Fact]
        public void Flow_WhenValidationFails_ExpectSingleErrorInResult3()
        {
            var result = _builder
                .For(new { Msg = "Mockery" })
                .Validate(x => false, () => new TestingError())
                .Finalize(x => false)
                .Sink(x => 1);
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
        public void Flow_WhenValidationFails_PipelineStopped()
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
        public void Flow_WhenValidationFails_PipelineStopped2()
        {
            bool executed = false;
            var result = _builder
                .For(new { Msg = "Mockery" })
                .Validate(x => x, x => false, () => new TestingError())
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
                .Validate(x => x.Prop, x => x, () => new TestingError())
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
                .Verify(x => false, () => new TestingError())
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
                .Verify(x => x.Msg, x => false, () => new TestingError())
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
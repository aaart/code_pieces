using Flow.Tests.TestUtilities;
using Xunit;

namespace Flow.Tests
{
    public partial class StandardFlowTests
    {
        private readonly StandardFlowBuilder<TestingFilteringError> _builder;

        public StandardFlowTests()
        {
            _builder = new StandardFlowBuilder<TestingFilteringError>();
        }

        [Fact]
        public void InputDoesMatter_WhenNoErrorsOccur_ExpectValidExecutionCount()
        {
            int count = 0;
            _builder
                .For(new { })
                .Validate(x =>
                {
                    count++;
                    return count == 1;
                }, () => new TestingFilteringError())
                .Apply(x =>
                {
                    count++;
                    return x;
                })
                .Verify(x =>
                {
                    count++;
                    return count == 3;
                }, () => new TestingFilteringError())
                .Verify(x =>
                {
                    count++;
                    return new { prop = x };
                }, x =>
                {
                    count++;
                    return count == 5;
                }, () => new TestingFilteringError())
                .Apply(x =>
                {
                    count++;
                    return count == 6;
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
            var (result, _, _) = _builder
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
    }
}
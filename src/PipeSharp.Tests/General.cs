using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public class General
    {
        private readonly IFlowBuilder<TestError> _predefinedFlow = Predefined.Flow;

        [Fact]
        public void InputDoesMatter_WhenNoErrorsOccur_ExpectValidExecutionCount()
        {
            int count = 0;
            _predefinedFlow
                .For(new { })
                .Check(x =>
                {
                    count++;
                    return count == 1;
                }, () => new TestError())
                .Apply(x =>
                {
                    count++;
                    return x;
                })
                .Check(x =>
                {
                    count++;
                    return count == 3;
                }, () => new TestError())
                .Check(x =>
                {
                    count++;
                    return new { prop = x };
                }, x =>
                {
                    count++;
                    return count == 5;
                }, () => new TestError())
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
            var (value, _) = _predefinedFlow
                .For(new { })
                .Check(x =>
                {
                    count++;
                    return true;
                }, () => new TestError())
                .Apply(x =>
                {
                    count++;
                    return x;
                })
                .Check(x =>
                {
                    count++;
                    return true;
                }, () => new TestError())
                .Check(x =>
                {
                    count++;
                    return new { prop = x };
                }, x =>
                {
                    count++;
                    return true;
                }, () => new TestError())
                .Apply(x =>
                {
                    count++;
                    return x;
                })
                .Finalize(x => count + 1)
                .Project(x => x + 1)
                .Sink();
            Assert.Equal(8, value);
        }

        [Fact]
        public void MockeryInput_WhenNoFiltersAndModificationsApplied_ExpectSuccessWithNoValueResult()
        {
            var summary = _predefinedFlow
                .For(new { Msg = "Mockery" })
                .Finalize(Predefined.EmptyMethod)
                .Sink();
            Assert.Empty(summary.Errors);
        }

        [Fact]
        public void DefultInt_WhenNoFiltersAndModificationsApplied_ExpectSuccessWithIntDefaultResult()
        {
            var input = default(int);
            var summary = _predefinedFlow
                .For(input)
                .Finalize(x => x)
                .Sink();
            Assert.Equal(input, summary.Value);
        }

        [Fact]
        public void One_WhenNoFiltersAndModificationsApplied_ExpectSuccessWithNumberOneResult()
        {
            var input = 1;
            var summary = _predefinedFlow
                .For(input)
                .Finalize(x => x)
                .Sink();
            Assert.Equal(input, summary.Value);
        }

        [Fact]
        public void DefaultIntAppliedOne_WhenNoFiltersAppliedButModificationsApplied_ExpectSuccessWithNumberOnResult()
        {
            var applied = 1;
            var summary = _predefinedFlow
                .For(0)
                .Apply(x => applied)
                .Finalize(x => x)
                .Sink();
            Assert.Equal(applied, summary.Value);
        }

        [Fact]
        public void DefaultIntAppliedOneAndTwo_WhenNoFiltersAppliedButModificationsApplied_ExpectSuccessWithNumberOnResult()
        {
            var applied = 2;
            var summary = _predefinedFlow
                .For(0)
                .Apply(x => 1)
                .Apply(x => applied)
                .Finalize(x => x)
                .Sink();
            Assert.Equal(applied, summary.Value);
        }

        [Fact]
        public void DefaultIn_WhenApplicationChangesType_ExpectSuccessWithEmptyString()
        {
            var applied = string.Empty;
            var summary = _predefinedFlow
                .For(0)
                .Apply(x => 1)
                .Apply(x => applied)
                .Finalize(x => x)
                .Sink();
            Assert.Equal(applied, summary.Value);
        }

        [Fact]
        public void DefaultIn_WhenApplicationChangesType2_ExpectSuccessWith21String()
        {
            var summary = _predefinedFlow
                .For(0)
                .Apply(x => x + 2)
                .Apply(x => x.ToString())
                .Apply(x => x.ToString() + "1")
                .Finalize(x => x)
                .Sink();
            Assert.Equal("21", summary.Value);
        }
    }
}
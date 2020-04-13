using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public partial class NotifyingFlowTests
    {
        [Fact]
        public void Flow_WhenOnDoingDefined_ExpectOnDoneNotRaised()
        {
            int onDoneCount = 0;
            var preDefined =
                new StandardBuilder()
                    .WithFilteringError<TestingFilteringError>()
                    .OnDone(() => { onDoneCount++; })
                    .WithEvents(new TestingEventReceiverFactory(() => {}, () => {}));
            

            preDefined.For(default(int))
                .Raise(x => new TestingEvent())
                .Finalize(x => { })
                .Sink();

            Assert.Equal(1, onDoneCount);
        }
    }
}
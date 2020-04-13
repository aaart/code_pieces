using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public partial class NotyfingFlowTests
    {
        [Fact]
        public void Flow_WhenOnDoingDefined_ExpectOnDoneNotRaised()
        {
            INotifyingFlowFactory<TestingFilteringError> factory =
                new NotifyingFlowFactory<TestingFilteringError>(new TestingEventReceiverFactory(() => { }, () => { }));
            int onDoneCount = 0;

            factory.For(default(int), () => {  }, () => { onDoneCount++; })
                .Raise(x => new TestingEvent())
                .Finalize(x => { })
                .Sink();

            Assert.Equal(1, onDoneCount);
        }
    }
}
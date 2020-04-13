using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public partial class NotyfingFlowTests
    {
        [Fact]
        public void Flow_WhenOnDoingDefined_ExpectOnDoingNotRaised()
        {
            INotifyingFlowFactory<TestingFilteringError> factory =
                new NotifyingFlowFactory<TestingFilteringError>(new TestingEventReceiverFactory(() => { }, () => { }));
            int onDoingCount = 0;

            factory.For(default(int), () => onDoingCount++, () => { })
                .Raise(x => new TestingEvent())
                .Finalize(x => { })
                .Sink();

            Assert.Equal(1, onDoingCount);
        }
    }
}
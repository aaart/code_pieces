using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public partial class NotifyingFlowTests
    {
        [Fact]
        public void Flow_WhenOnDoingDefined_ExpectOnDoingNotRaised()
        {
            int onDoingCount = 0;
            var preDefined =
                new StandardBuilder()
                    .WithFilteringError<TestingFilteringError>()
                    .OnDoing(() => onDoingCount++)
                    .WithEvents(new TestingEventReceiverFactory(() => { }, () => { }));
            

            preDefined.For(default(int))
                .Raise(x => new TestingEvent())
                .Finalize(x => { })
                .Sink();

            Assert.Equal(1, onDoingCount);
        }
    }
}
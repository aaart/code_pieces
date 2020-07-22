using System;

namespace PipeSharp.Tests.TestUtilities
{
    public class TestingSubscription : ISubscription
    {
        private readonly Action _onReceived;
        private readonly Action _onDisposing;

        public TestingSubscription(Action onReceived, Action onDisposing)
        {
            _onReceived = onReceived;
            _onDisposing = onDisposing;
        }

        public IActiveSubscription Subscribe() => new TestingEventSubscription(_onReceived, _onDisposing);
    }
}
using System;

namespace PipeSharp.Tests.TestUtilities
{
    public class TestingEventReceiverFactory : IEventReceiverFactory
    {
        private readonly Action _onReceived;
        private readonly Action _onDisposing;

        public TestingEventReceiverFactory(Action onReceived, Action onDisposing)
        {
            _onReceived = onReceived;
            _onDisposing = onDisposing;
        }

        public IEventReceiver Create() => new TestingEventReceiver(_onReceived, _onDisposing);
    }
}
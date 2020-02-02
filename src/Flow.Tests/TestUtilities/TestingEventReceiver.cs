using System;

namespace PipeSharp.Tests.TestUtilities
{
    public class TestingEventReceiver : IEventReceiver
    {
        private readonly Action _onReceived;
        private readonly Action _onDisposing;

        public TestingEventReceiver(Action onReceived, Action onDisposing)
        {
            _onReceived = onReceived;
            _onDisposing = onDisposing;
        }

        public void Receive<TE>(TE e) => _onReceived();

        public void Dispose() => _onDisposing();
    }
}
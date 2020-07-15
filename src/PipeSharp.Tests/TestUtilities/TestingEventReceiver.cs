using System;

namespace PipeSharp.Tests.TestUtilities
{
    public class TestingEventReceiver : AbstractEventReceiver
    {
        private readonly Action _onReceived;
        private readonly Action _onDisposing;

        public TestingEventReceiver(Action onReceived, Action onDisposing)
        {
            _onReceived = onReceived;
            _onDisposing = onDisposing;
        }

        public override void Receive<TE>(TE e) => _onReceived();

        public override void Dispose() => _onDisposing();
    }
}
using System;

namespace Flow.Tests.TestUtilities
{
    public class TestingEventReceiver : IEventReceiver
    {
        private readonly Action _onReceived;

        public TestingEventReceiver(Action onReceived)
        {
            _onReceived = onReceived;
        }

        public void Receive<TE>(TE e) where TE : IEvent
        {
            _onReceived();
        }
    }
}
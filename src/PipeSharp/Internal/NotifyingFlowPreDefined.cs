using System;
using Microsoft.Extensions.Logging;

namespace PipeSharp.Internal
{
    public class NotifyingFlowPreDefined<TFilteringError> : INotifyingFlowPreDefined<TFilteringError>
    {
        private readonly IEventReceiverFactory _eventReceiverFactory;
        private readonly ILogger _logger;
        private readonly Action _onDoing;
        private readonly Action _onDone;

        public NotifyingFlowPreDefined(ILogger logger, IEventReceiverFactory eventReceiverFactory, Action onDoing, Action onDone)
        {
            _logger = logger;
            _eventReceiverFactory = eventReceiverFactory;
            _onDoing = onDoing;
            _onDone = onDone;
        }
        public INotifyingFlow<T, TFilteringError> For<T>(T target) =>
            new Step<T, TFilteringError>(
                () => new State<T, TFilteringError>(target, new StateData<TFilteringError>(_logger, _eventReceiverFactory.Create())),
                _onDoing,
                _onDone);
    }
}
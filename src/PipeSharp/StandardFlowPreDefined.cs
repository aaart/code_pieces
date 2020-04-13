using System;
using Microsoft.Extensions.Logging;
using PipeSharp.Internal;

namespace PipeSharp
{
    public class StandardFlowPreDefined<TFilteringError> : IFlowPreDefined<TFilteringError>
    {
        private readonly ILogger _logger;
        private readonly Action _onDoing;
        private readonly Action _onDone;

        public StandardFlowPreDefined(ILogger logger, Action onDoing, Action onDone)
        {
            _logger = logger;
            _onDoing = onDoing;
            _onDone = onDone;
        }
        public IFlow<T, TFilteringError> For<T>(T target) =>
            new Step<T, TFilteringError>(
                () => new State<T, TFilteringError>(target, new StateData<TFilteringError>(_logger, new OutNullEventReceiver())),
                _onDoing,
                _onDone);
    }
}
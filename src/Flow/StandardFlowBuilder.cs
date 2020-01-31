namespace Flow
{
    public class StandardFlowBuilder<TFilteringError> : IFlowBuilder<TFilteringError>
    {
        public IFlow<T, TFilteringError> For<T>(T target) => 
            new Step<T, TFilteringError>(() => new State<T, TFilteringError>(target, new StateData<TFilteringError>(new BlackholeEventReceiver())));
    }
}
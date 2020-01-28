namespace Flow
{
    public class StandardFlowBuilder : IFlowBuilder
    {
        public IFlow<T> For<T>(T target) => 
            new Step<T>(() => new StepResult<T, BlackholeEventReceiver>(target, new StepState<BlackholeEventReceiver>(new BlackholeEventReceiver(), (e, er) => { })));
    }
}
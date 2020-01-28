namespace Flow
{
    public class StandardFlowBuilder : IFlowBuilder
    {
        public IFlow<T> For<T>(T target) => 
            new Step<T>(() => new State<T>(target, new StateData(new BlackholeEventReceiver())));
    }
}
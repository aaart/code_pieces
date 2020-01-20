namespace Flow
{
    public class StandardPipelineBuilder : IPipelineBuilder
    {
        public IBeginFlow<T> For<T>(T target) => 
            new Step<T>(() => new State<T, StandardEventReceiver>(target, new StandardEventReceiver()));
    }
}
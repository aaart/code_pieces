namespace Flow
{
    public class StandardPipelineBuilder : IPipelineBuilder
    {
        public IBeginFlow<T> For<T>(T target) => 
            new FlowItem<T>(() => new StateStack(target));
    }
}
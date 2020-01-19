namespace Flow
{
    public class StandardPipelineBuilder : IPipelineBuilder
    {
        public IBeginFlow<T> For<T>(T target) => 
            new Step<T>(() => new State<T>(target));
    }
}
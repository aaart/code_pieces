namespace Flow
{
    public class PipelineResult : IPipelineResult
    {
       public bool Failed { get; set; }
    }
    public class PipelineResult<T> : PipelineResult, IPipelineResult<T>
    {
        public T Value { get; set; }
    }
}
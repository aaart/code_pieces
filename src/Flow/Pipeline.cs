using System;

namespace Flow
{
    public class Pipeline : IPipeline
    {
        protected Action<IFlowItemToken> Action { get; }

        internal Pipeline(Action<IFlowItemToken> action)
        {
            Action = action;
        }

        public IPipelineResult Sink()
        {
            var token = new FlowItemTokenStack();
            Action(token);
            var result = new PipelineResult();
            result.Errors.AddRange(token.Errors);
            return result;
        }
    }

    public class Pipeline<T> : Pipeline, IPipeline<T>
    {
        internal Pipeline(Action<IFlowItemToken> action)
            : base(action)
        {
        }

        public IPipelineResult<TR> Sink<TR>(Func<T, TR> projection)
        {
            throw new NotImplementedException();
        }

        public new IPipelineResult<T> Sink()
        {
            var pass = new FlowItemTokenStack();
            Action(pass);
            return new PipelineResult<T> { Result = (T)pass.CurrentResult };
        }
    }
}
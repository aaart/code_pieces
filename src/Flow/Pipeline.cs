using System;

namespace Flow
{
    public class Pipeline : IPipeline
    {
        protected Action<IFlowItemState> Action { get; }

        internal Pipeline(Action<IFlowItemState> action)
        {
            Action = action;
        }

        public IPipelineResult Sink()
        {
            var token = new FlowItemStateStack();
            Action(token);
            var result = new PipelineResult();
            result.Errors.AddRange(token.Errors);
            return result;
        }
    }

    public class Pipeline<T> : Pipeline, IPipeline<T>
    {
        internal Pipeline(Action<IFlowItemState> action)
            : base(action)
        {
        }

        public IPipelineResult<TR> Sink<TR>(Func<T, TR> projection)
        {
            throw new NotImplementedException();
        }

        public new IPipelineResult<T> Sink()
        {
            var pass = new FlowItemStateStack();
            Action(pass);
            return new PipelineResult<T> { Result = (T)pass.CurrentResult };
        }
    }
}
using System;
using System.Linq;

namespace Flow
{
    public class Pipeline : IPipeline
    {
        protected Func<IFlowItemState> Method { get; }

        internal Pipeline(Func<IFlowItemState> method)
        {
            Method = method;
        }

        public IPipelineResult Sink() => CreateResult<PipelineResult>();

        protected T CreateResult<T>(Action<T, IFlowItemState> setup = null)
            where T : PipelineResult, new()
        {
            var state = Method();
            var result = new T();
            result.Errors.AddRange(state.Errors);
            setup?.Invoke(result, state);
            state.EventReceiver.Dispose();
            return result;
        }
    }

    public class Pipeline<T> : Pipeline, IPipeline<T>
    {
        internal Pipeline(Func<IFlowItemState> method)
            : base(method)
        {
        }

        public IPipelineResult<TR> Sink<TR>(Func<T, TR> projection) =>
            CreateResult<PipelineResult<TR>>((result, state) =>
            {
                if (!state.Errors.Any())
                {
                    result.Result = projection((T)state.CurrentResult);
                }
            });

        public new IPipelineResult<T> Sink() => Sink(x => x);

    }
}
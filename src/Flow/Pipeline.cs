using System;
using System.Collections;
using System.Collections.Generic;

namespace Flow
{
    public class Pipeline : IPipeline
    {
        protected List<Action<Stack>> Actions { get; }
        protected Stack ResultStack { get; }

        internal Pipeline(List<Action<Stack>> actions, Stack resultStack)
        {
            Actions = actions;
            ResultStack = resultStack;
        }

        public IPipelineResult Sink()
        {
            foreach (Action<Stack> action in Actions)
            {
                action(ResultStack);
            }
            return new PipelineResult();
        }
    }

    public class Pipeline<T> : Pipeline, IPipeline<T>
    {
        

        internal Pipeline(List<Action<Stack>> actions, Stack resultStack)
            : base(actions, resultStack)
        {
        }

        public IPipelineResult<TR> Sink<TR>(Func<T, TR> projection)
        {
            throw new NotImplementedException();
        }

        public new IPipelineResult<T> Sink()
        {
            foreach (Action<Stack> action in Actions)
            {
                action(ResultStack);
            }
            return new PipelineResult<T>{ Result = (T)ResultStack.Peek()};
        }
    }
}
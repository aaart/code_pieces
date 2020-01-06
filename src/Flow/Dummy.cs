using System;
using System.Linq.Expressions;

namespace Flow
{
    public class DummyError : IError
    {
        public int Code => 1;
        public string Message => "MSG";
    }

    public class Dummy
    {
        public void Method()
        {

            var target = new { Name = "Random", Description = "More Random" };
            var pipeline = new StandardPipelineBuilder()
                .For(target)
                .Validate(x => x.Description, x => x != null, () => new DummyError())
                .Apply(x => new { Wrapper = x })
                .Verify(x => x.Wrapper, x => x != null, () => new DummyError())
                .Finalize(x => Console.Write("x"));
            var res = pipeline.Sink();

            new StandardPipelineBuilder().For(target).Finalize(x => new { Result = x });

            Expression<Func<bool>> expr = () => true;
        }
    }
}
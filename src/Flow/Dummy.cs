using System;
using System.Linq.Expressions;

namespace Flow
{
    public class DummyFilteringError : IFilteringError
    {
        public int Code => 1;
        public string Message => "MSG";
    }

    public class Dummy
    {
        public void Method()
        {

            var target = new { Name = "Random", Description = "More Random" };
            var pipeline = new StandardFlowBuilder()
                .For(target)
                .Validate(x => x.Description, x => x != null, () => new DummyFilteringError())
                .Apply(x => new { Wrapper = x })
                .Verify(x => x.Wrapper, x => x != null, () => new DummyFilteringError())
                .Finalize(x => Console.Write("x"));
            var res = pipeline.Sink();

            new StandardFlowBuilder().For(target).Finalize(x => new { Result = x });

            Expression<Func<bool>> expr = () => true;
        }
    }
}
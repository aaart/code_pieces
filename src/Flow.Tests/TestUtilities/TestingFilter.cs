namespace PipeSharp.Tests.TestUtilities
{
    public class TestingFilter : IFilter<TestingInput, TestingFilteringError>
    {
        public bool Check(TestingInput target, out TestingFilteringError filteringError)
        {
            filteringError = new TestingFilteringError();
            return false;
        }
    }
}
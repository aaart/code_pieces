namespace Flow.Tests.TestUtilities
{
    public class TestingFilter : IFilter<TestingInput>
    {
        public bool Check(TestingInput target, out IFilteringError filteringError)
        {
            filteringError = new TestingFilteringError();
            return false;
        }
    }
}
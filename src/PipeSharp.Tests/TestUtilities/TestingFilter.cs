namespace PipeSharp.Tests.TestUtilities
{
    public class TestingFilter : IFilter<TestingInput, TestError>
    {
        public bool Check(TestingInput target, out TestError filteringError)
        {
            filteringError = new TestError();
            return false;
        }
    }
}
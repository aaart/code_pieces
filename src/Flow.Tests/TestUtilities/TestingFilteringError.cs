namespace Flow.Tests.TestUtilities
{
    public struct TestingFilteringError : IFilteringError
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
namespace Flow.Tests.TestUtilities
{
    public struct TestingError : IError
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
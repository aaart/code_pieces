namespace PipeSharp.Tests.TestUtilities
{
    public static class Predefined
    {
        public static IFlowBuilder<TestError> Flow => 
            new StandardBuilder()
                .UseErrorType<TestError>(ex => new TestError{Code = ex.HResult, Message = ex.Message});
        
        public static void EmptyMethod() { }
        public static void EmptyMethod<T>(T target) { }

        public static T DefaultOf<T>() => default(T);
        public static T DefaultOf<T>(object x) => default(T);

    }
}
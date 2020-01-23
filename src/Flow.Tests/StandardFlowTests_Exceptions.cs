using System;
using Xunit;

namespace Flow.Tests
{
    public partial class StandardFlowTests
    {
        [Fact]
        public void StandardFlow_WhenFinalizeThrowsException_ExceptionReturned()
        {
            var result = _builder
                .For(default(int))
                .Finalize(x =>
                {
                    throw new Exception();
                    return x;
                })
                .Sink();

            Assert.NotNull(result.Exception);
        }
    }
}
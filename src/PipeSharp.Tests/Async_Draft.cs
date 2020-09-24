using System;
using System.Threading.Tasks;
using PipeSharp.Tests.TestUtilities;
using RestSharp;
using Xunit;

namespace PipeSharp.Tests
{
    public class Async_Draft
    {
        private readonly IFlowBuilder<TestError> _predefinedFlow = Predefined.Flow;
        
        [Fact(Skip = "Async not working yet.")]
        public async Task AsyncPipeline_NoError_ExpectExecution()
        {
            var summary = _predefinedFlow
                .For(1000)
                .Finalize(async x => await Task.Delay(x))
                .Sink();

            await summary.Value;
            //Assert.Null(summary.Exception);
        }
        
        [Fact(Skip = "Async not working yet.")]
        public async Task AsyncPipeline_ExceptionThrown_ExpectExceptionInResult()
        {
            var summary = _predefinedFlow
                .For("https://google.com")
                .Apply(async x =>
                {
                    
                    throw new Exception();
#pragma warning disable 162
                    var client = new RestClient(x);
                    var request = new RestRequest(Method.GET);
                    var response = await client.ExecuteGetAsync(request);
                    return response;
#pragma warning restore 162
                })
                .Finalize(async x => await x)
                .Sink();

            await summary.Value;
            //Assert.NotNull(summary.Exception);
        }

        [Fact(Skip = "Async not working yet.")]
        public async Task Blah()
        {
            async Task Action()
            {
                throw new Exception();
#pragma warning disable 162
                await Task.Delay(9);
#pragma warning restore 162
            }

            await Action();
        }
    }
}
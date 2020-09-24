﻿using PipeSharp.Tests.TestUtilities;
using Xunit;

namespace PipeSharp.Tests
{
    public class OnChanging
    {
        private readonly IFlowBuilder<TestError> _predefinedFlow = Predefined.Flow;
        
        [Fact]
        public void GivenFlow_WhenOnlyFinalizeDefined_ExpectSingleOnDoing()
        {
            int onDoingCount = 0;
            _predefinedFlow
                .OnChanging(() => onDoingCount++)
                .EnableEventSubscription(new TestingSubscription(() => onDoingCount++, () => {}))
                .OnChanging(() => onDoingCount++)
                .For(default(int))
                .Finalize(x => {})
                .Sink();
            Assert.Equal(2, onDoingCount);
        }
        
        [Fact]
        public void GivenFlow_WhenApplyAndFinalizeDefined_ExpectSingleOnDoing()
        {
            int onDoingCount = 0;
            _predefinedFlow
                .OnChanging(() => onDoingCount++)
                .EnableEventSubscription(new TestingSubscription(() => onDoingCount++, () => { }))
                .OnChanging(() => onDoingCount++)
                .For(default(int))
                .Apply(x => x)
                .Apply(x => x)
                .Apply(x => x)
                .Apply(x => x)
                .Apply(x => x)
                .Apply(x => x)
                .Apply(x => x)
                .Apply(x => x)
                .Apply(x => x)
                .Finalize(x => {})
                .Sink();
            Assert.Equal(20, onDoingCount);
        }

        [Fact]
        public void GivenFlow_WhenCheckingDefined_ExpectSingleOnDoing()
        {
            int onDoingCount = 0;
            _predefinedFlow
                .OnChanging(() => onDoingCount++)
                .EnableEventSubscription(new TestingSubscription(() => onDoingCount++, () => { }))
                .OnChanging(() => onDoingCount++)
                .For(default(int))
                .Check(x => true, () => new TestError())
                .Finalize(x => { })
                .Sink();
            Assert.Equal(2, onDoingCount);
        }
    }
}
using System;

namespace PipeSharp
{
    public class Pipeline<TFilteringError> : IPipeline<TFilteringError>
    {
        private readonly Func<IState<TFilteringError>> _method;
        internal Pipeline(Func<IState<TFilteringError>> method)
        {
            _method = method;
        }

        public (IResult, Exception, TFilteringError[]) Sink() => 
            _method().Sink<Result, IState<TFilteringError>, TFilteringError>((result, state) =>
            {
                result.Failed = state.Invalid || state.Broken;
            });
    }

    public class Pipeline<T, TFilteringError> : IProjectablePipeline<T, TFilteringError>
    {
        private readonly Func<IState<T, TFilteringError>> _method;

        internal Pipeline(Func<IState<T, TFilteringError>> method)
        {
            _method = method;
        }

        public IProjectablePipeline<TR, TFilteringError> Project<TR>(Func<T, TR> projection) => 
            new Pipeline<TR, TFilteringError>(() => _method.Decorate(state => projection(state.Result)));

        public (IResult<T>, Exception, TFilteringError[]) Sink() =>
            _method().Sink<Result<T>, IState<T, TFilteringError>, TFilteringError>((result, state) =>
            {
                result.Failed = state.Invalid || state.Broken;
                if (!result.Failed)
                {
                    result.Value = state.Result;
                }
            });

        (IResult, Exception, TFilteringError[]) IPipeline<TFilteringError>.Sink()
        {
            return Sink();
        }
    }
}
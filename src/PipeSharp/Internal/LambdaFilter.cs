using System;

namespace PipeSharp
{
    public class LambdaFilter<T, TFilteringError> : IFilter<T, TFilteringError>
    {
        private readonly Func<T, bool> _check;
        private readonly Func<TFilteringError> _errorFunc;

        public LambdaFilter(Func<T, bool> check, Func<TFilteringError> errorFunc)
        {
            _check = check;
            _errorFunc = errorFunc;
        }

        public bool Check(T target, out TFilteringError filteringError)
        {
            filteringError = default;
            if (!_check(target))
            {
                filteringError = _errorFunc();
                return false;
            }

            return true;
        }
    }
}
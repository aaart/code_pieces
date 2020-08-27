using System;

namespace PipeSharp.Internal
{
    public class LambdaFilter<T, TError> : IFilter<T, TError>
    {
        private readonly Func<T, bool> _check;
        private readonly Func<TError> _errorFunc;

        public LambdaFilter(Func<T, bool> check, Func<TError> errorFunc)
        {
            _check = check;
            _errorFunc = errorFunc;
        }

        public bool Check(T target, out TError filteringError)
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
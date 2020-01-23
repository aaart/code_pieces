using System;

namespace Flow
{
    public class LambdaFilter<T> : IFilter<T>
    {
        private readonly Func<T, bool> _check;
        private readonly Func<IFilteringError> _errorFunc;

        public LambdaFilter(Func<T, bool> check, Func<IFilteringError> errorFunc)
        {
            _check = check;
            _errorFunc = errorFunc;
        }

        public bool Check(T target, out IFilteringError filteringError)
        {
            filteringError = null;
            if (!_check(target))
            {
                filteringError = _errorFunc();
                return false;
            }

            return true;
        }
    }
}
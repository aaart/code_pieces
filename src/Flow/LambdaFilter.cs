using System;

namespace Flow
{
    public class LambdaFilter<T> : IFilter<T>
    {
        private readonly Func<T, bool> _check;
        private readonly Func<IError> _errorFunc;

        public LambdaFilter(Func<T, bool> check, Func<IError> errorFunc)
        {
            _check = check;
            _errorFunc = errorFunc;
        }

        public bool Check(T target, out IError error)
        {
            error = null;
            if (!_check(target))
            {
                error = _errorFunc();
                return false;
            }

            return true;
        }
    }
}
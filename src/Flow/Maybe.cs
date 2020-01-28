using System;
using System.Collections.Generic;

namespace Flow
{
    //public class Maybe
    //{
    //    public Maybe()
    //    {
    //        IsValid = false;
    //    }
        
    //}

    public class Maybe<T>// : Maybe
    {
        public Maybe()
        {
            HasValue = false;
        }
        public Maybe(T value)
        {
            HasValue = true;
            Value = value;
        }
        public bool HasValue { get; protected set; }
        public T Value { get; }
    }
}
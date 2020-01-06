using System;

namespace Flow.Tests.TestUtilities
{
    public static class Predefined
    {
        public static void EmptyMethod() { }
        public static void EmptyMethod<T>(T target) { }

        public static T DefaultOf<T>() => default(T);
        public static T DefaultOf<T>(object x) => default(T);

    }
}
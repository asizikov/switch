using System;
using System.Linq.Expressions;

namespace Switch.Read
{
    internal static class CustomActivator
    {
        public static T CreateInstance<T>() where T : new() => ActivatorImpl<T>.Factory();

        private class ActivatorImpl<T> where T : new()
        {
            private static readonly 
                Expression<Func<T>> Expr = () => new T();
            public static readonly Func<T> Factory = Expr.Compile();
        }
    }
}
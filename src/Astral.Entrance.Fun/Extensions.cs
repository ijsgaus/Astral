using System;
using System.Reflection;
using LanguageExt;
using LanguageExt.TypeClasses;

namespace Astral.Fun
{
    public static class Extensions
    {
        public static T Unwrap<T>(this Option<T> option, Func<Option<T>, string> onError)
        {
            return option.Match(p => p, () => throw new InvalidOperationException(onError(option)));
        }

        public static T Unwrap<T>(this Option<T> option, Func<string> onError)
        {
            return Unwrap(option, p => onError());
        }

        public static T Unwrap<T>(this Option<T> option, string onError)
        {
            return Unwrap(option, p => onError);
        }

        public static T Unwrap<T>(this Option<T> option)
        {
            return Unwrap(option, p => $"Unwrap None of type {typeof(T)}");
        }

        public static Option<T> UnboxUnsafe<T>(this Option<object> option)
        {
            return option.Map(p => (T) p);
        }

        public static Option<T> Unbox<T>(this Option<object> option)
        {
            return option.Bind(p => p is T v ? Prelude.Some(v) : Prelude.None);
        }

        public static Option<object> Box<T>(this Option<T> option)
        {
            return option.Map(p => (object) p);
        }

        public static T Check<T>(this Pred<T> predicate, T value)

        {
            if (predicate.True(value)) return value;
            if(predicate is IPredError<T> er)
                throw new PredException(er.GetError(value));
            var attr = predicate.GetType().GetTypeInfo().GetCustomAttribute<PredErrorAttribute>();
            if(attr != null)
                throw new PredException(string.Format(attr.Format, value));
            throw new PredException($"{value} not satisfy predicate {predicate.GetType().Name}");
        }
    }
}
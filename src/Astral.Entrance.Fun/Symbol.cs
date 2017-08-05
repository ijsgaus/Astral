using System;
using System.Collections.Generic;
using LanguageExt.TypeClasses;

namespace Astral.Fun
{
    public abstract class Symbol<T> : ISymbol<T>, IEquatable<Symbol<T>>
    {
        protected Symbol(T value)
        {
            Value = value;
        }

        Type ISymbol.Type => typeof(T);

        object ISymbol.UnsafeValue => Value;

        public T Value { get; }

        public virtual bool Equals(Symbol<T> other)
        {
            if (other == null) return false;
            return other.GetType() == GetType() && Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Symbol<T>;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() * 27 + EqualityComparer<T>.Default.GetHashCode(Value);
        }
    }

    public abstract class Symbol<T, TEq> : Symbol<T>
        where TEq : struct, Eq<T>
    {
        protected Symbol(T value) : base(value)
        {
        }

        public override bool Equals(Symbol<T> other)
        {
            if (other == null) return false;
            return other.GetType() == GetType() && default(TEq).Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() * 27 + default(TEq).GetHashCode(Value);
        }
    }

    public abstract class Symbol<T, TEq, TPred> : Symbol<T, TEq>
        where TPred : struct, Pred<T> 
        where TEq : struct, Eq<T>
    {
        protected Symbol(T value) : base(value)
        {
            default(TPred).Check(value);
        }
    }
}
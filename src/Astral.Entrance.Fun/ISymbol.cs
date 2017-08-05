using System;

namespace Astral.Fun
{
    public interface ISymbol
    {
        Type Type { get; }
        object UnsafeValue { get; }
    }

    public interface ISymbol<T> : ISymbol
    {
        T Value { get; }
    }
}
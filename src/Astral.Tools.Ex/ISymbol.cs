using System;

namespace Astral.Tools.Ex
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
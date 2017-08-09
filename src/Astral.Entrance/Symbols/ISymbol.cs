using System;

namespace Astral.Symbols
{
    public interface ISymbol
    {
        string Name { get; }
        Type Type { get; }
    }

    public interface ISymbol<T> : ISymbol
    {
        
    }
}
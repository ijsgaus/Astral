using System;

namespace Astral.Symbols
{
    public abstract class SymbolAttribute : Attribute, ISymbol
    {
        protected SymbolAttribute(string name, Type type)
        {
            _name = name;
            _type = type;
        }

        private readonly string _name;
        private readonly Type _type;

        string ISymbol.Name => _name;

        Type ISymbol.Type => _type;
        
    }

    public abstract class ExtensionSymbolAttribute : SymbolAttribute
    {
        protected ExtensionSymbolAttribute(string name, Type type, string extensionName) : base(name, type)
        {
            ExtensionName = extensionName;
        }
        
        public string ExtensionName { get; }
    }
}
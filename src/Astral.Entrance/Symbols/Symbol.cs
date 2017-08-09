using System;

namespace Astral.Symbols
{
    public class Symbol<T> : ISymbol<T> 
    {
        public Symbol(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public Type Type => typeof(T);


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as ISymbol;
            if (other == null) return false;
            return other.Type == Type && Equals(other.Name, Name);
        }

        public override int GetHashCode()
        {
            return (Name?.GetHashCode() ?? 0) * 27 + Type.GetHashCode();
        }

        public static bool operator ==(Symbol<T> left, ISymbol right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Symbol<T> left, ISymbol right)
        {
            return !Equals(left, right);
        }
        
        public static bool operator ==(ISymbol left, Symbol<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ISymbol left, Symbol<T> right)
        {
            return !Equals(left, right);
        }
    }
}
using System;

namespace Astral.Runes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContractNameAttribute : Attribute
    {
        public ContractNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
using System;

namespace Astral
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ContractAttribute : Attribute
    {
        public ContractAttribute(string version, string name = null)
        {
            Name = name;
            Version = Version.Parse(version);
        }

        public string Name { get; }
        public Version Version { get; }
    }
}
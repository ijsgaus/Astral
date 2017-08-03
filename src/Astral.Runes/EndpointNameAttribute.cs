using System;

namespace Astral.Runes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EndpointNameAttribute : Attribute
    {
        public EndpointNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
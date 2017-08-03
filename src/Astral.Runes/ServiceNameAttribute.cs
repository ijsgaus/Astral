using System;

namespace Astral.Runes
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class ServiceNameAttribute : Attribute
    {
        public ServiceNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
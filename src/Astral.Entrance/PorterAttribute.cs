using System;

namespace Astral
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Property, AllowMultiple = true)]
    public class PorterAttribute : Attribute
    {
        public PorterAttribute(PorterType type, string code)
        {
            Type = type;
            Code = code;
        }

        public PorterType Type { get;  }
        public string Code { get; }
    }
}
using System;

namespace Astral
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Property, AllowMultiple = true)]
    public class TransportAttribute : Attribute
    {
        public TransportAttribute(TransportType type, string code)
        {
            Type = type;
            Code = code;
        }

        public TransportType Type { get;  }
        public string Code { get; }
    }
}
using System;

namespace Astral.Rabbit
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Property)]
    public class ExchangeInAttribute : Attribute
    {
        public ExchangeInAttribute(ExchangeType type = ExchangeType.Direct, string name = null)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public ExchangeType Type { get; }
    }
}
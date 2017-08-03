using System;

namespace Astral.Runes.Rabbit
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Property)]
    public class ExchangeAttribute : Attribute
    {
        public ExchangeAttribute(string name, BusExchangeType type = BusExchangeType.Direct)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public BusExchangeType Type { get; }
    }
}
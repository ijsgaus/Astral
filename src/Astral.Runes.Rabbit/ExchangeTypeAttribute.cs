using System;

namespace Astral.Runes.Rabbit
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class ExchangeTypeAttribute : Attribute
    {
        public ExchangeTypeAttribute(BusExchangeType type)
        {
            Type = type;
        }

        public BusExchangeType Type { get; }
    }
}
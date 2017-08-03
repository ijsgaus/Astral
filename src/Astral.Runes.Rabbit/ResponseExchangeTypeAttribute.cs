using System;

namespace Astral.Runes.Rabbit
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class ResponseExchangeTypeAttribute : Attribute
    {
        public ResponseExchangeTypeAttribute(BusExchangeType type)
        {
            Type = type;
        }

        public BusExchangeType Type { get; }
    }
}
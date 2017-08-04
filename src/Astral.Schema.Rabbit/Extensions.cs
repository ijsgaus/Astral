using Astral.Runes.Rabbit;

namespace Astral.Schema.Rabbit
{
    public static class Extensions
    {
        public static string ToJsonString(this BusExchangeType exchangeType)
            => exchangeType.ToString().ToLower();
    }
}
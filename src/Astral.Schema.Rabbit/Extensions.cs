using Astral.Runes.Rabbit;

namespace Astral.Schema.Rabbit
{
    public static class Extensions
    {
        public static string ToJsonString(this ExchangeType exchangeType)
            => exchangeType.ToString().ToLower();
        
        
    }
}
using System.Collections.Generic;

namespace Astral.Schema
{
    public class GatePartSchema : SchemaBase
    {
        public IDictionary<PorterType, string> Porters { get; set; } = new Dictionary<PorterType, string>();

        public bool ShouldSerializeTransports()
        {
            return Porters.Count > 0;
        }
    }
}
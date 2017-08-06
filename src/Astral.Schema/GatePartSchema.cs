using System.Collections.Generic;

namespace Astral.Schema
{
    public class GatePartSchema : SchemaBase
    {
        public IDictionary<TransportType, string> Transports { get; set; } = new Dictionary<TransportType, string>();

        public bool ShouldSerializeTransports()
        {
            return Transports.Count > 0;
        }
    }
}
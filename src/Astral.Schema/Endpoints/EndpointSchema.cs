using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Astral.Schema
{
    public abstract class EndpointSchema : GatePartSchema
    {
        [JsonProperty(IsReference = true)]
        public ServiceSchema Owner { get; set; }
    }
}
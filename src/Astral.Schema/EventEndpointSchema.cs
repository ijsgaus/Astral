using Newtonsoft.Json;
using NJsonSchema;

namespace Astral.Schema
{
    public class EventEndpointSchema : EndpointSchema
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public ContractTypeSchema Event { get; set; }
    }
}
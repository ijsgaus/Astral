using Newtonsoft.Json;

namespace Astral.Schema
{
    public class CommandEndpointSchema : EndpointSchema
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public ContractTypeSchema Command { get; set; }
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Astral.Schema
{
    public class ArrayTypeSchema : ContractTypeSchema
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public ContractTypeSchema Element { get; set; } 
    }
}
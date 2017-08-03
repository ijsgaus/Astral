using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Astral.Schema
{
    public class ContractTypeSchema
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public JObject Schema { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
    }
}
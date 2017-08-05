using System.Net.Http.Headers;
using static LanguageExt.Prelude;

namespace Astral.Schema.Rabbit
{
    internal class ServiceSchemaExtension : SchemaExtension<ServiceSchema>
    {
        public ServiceSchemaExtension(ServiceSchema schema) : base(schema)
        {
        }
    }
}
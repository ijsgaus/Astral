using System;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Astral.Schema.Generation
{
    public interface ISchemaGeneratorExtension
    {
        void ExtendService(Type serviceType, ServiceSchema schema);
        void ExtendEndpoint(PropertyInfo propertyInfo, EndpointSchema schema);
    }
}
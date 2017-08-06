using System;
using System.Reflection;

namespace Astral.Schema
{
    public interface ISchemaGeneratorExtension
    {
        void ExtendService(Type serviceType, ServiceSchema schema);
        void ExtendEndpoint(PropertyInfo propertyInfo, EndpointSchema schema);
    }
}
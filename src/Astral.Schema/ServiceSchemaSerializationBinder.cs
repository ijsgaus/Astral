using System;
using Newtonsoft.Json.Serialization;

namespace Astral.Schema
{
    public class ServiceSchemaSerializationBinder : ISerializationBinder
    {
        public Type BindToType(string assemblyName, string typeName)
        {
            switch (typeName)
            {
                case "event":
                    return typeof(EventEndpointSchema);
                case "command":
                    return typeof(CommandEndpointSchema);
                case "callable":
                    return typeof(CallableEndpointSchema);
                case "objectType":
                    return typeof(ObjectTypeSchema);
                case "objectHierarchy":
                    return typeof(HierarchyTypeSchema);
                case "primitiveType":
                    return typeof(PrimitiveTypeSchema);
                case "arrayType":
                    return typeof(ArrayTypeSchema);
                default:
                    throw new InvalidOperationException($"Unknow type {assemblyName} {typeName}");
            }
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            if (serializedType == typeof(EventEndpointSchema))
            {
                typeName = "event";
                return;
            }
            if (serializedType == typeof(CommandEndpointSchema))
            {
                typeName = "command";
                return;
            }
            if (serializedType == typeof(CallableEndpointSchema))
            {
                typeName = "callable";
                return;
            }
            if (serializedType == typeof(ObjectTypeSchema))
            {
                typeName = "objectType";
                return;
            }
            if (serializedType == typeof(HierarchyTypeSchema))
            {
                typeName = "objectHierarchy";
                return;
            }
            if (serializedType == typeof(PrimitiveTypeSchema))
            {
                typeName = "primitiveType";
                return;
            }
            if (serializedType == typeof(ArrayTypeSchema))
            {
                typeName = "arrayType";
                return;
            }
            throw new InvalidOperationException($"Unknow type {serializedType}");
        }
    }
}
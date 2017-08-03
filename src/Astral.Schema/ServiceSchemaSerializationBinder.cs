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
            throw new InvalidOperationException($"Unknow type {serializedType}");
        }
    }
}
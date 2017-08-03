using System;
using System.Linq;
using System.Reflection;
using Astral.Runes;
using Astral.Schema.Exceptions;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using NJsonSchema.Generation;

namespace Astral.Schema.Generation
{
    public class SchemaGenerator<T>
    {
        private readonly ISchemaGeneratorExtension[] _extensions;
        private readonly bool _useOnlyAttributes;
        private readonly TypeInfo _serviceTypeInfo;

        public SchemaGenerator(ISchemaGeneratorExtension[] extensions = null, bool useOnlyAttributes = true)
        {
            if(!typeof(T).GetTypeInfo().IsInterface) throw new ArgumentException($"Shema type must be interface");
            _extensions = extensions ?? new ISchemaGeneratorExtension[0];
            _useOnlyAttributes = useOnlyAttributes;
            _serviceTypeInfo = typeof(T).GetTypeInfo();
        }

        public ServiceSchema Generate(Version version = null)
        {
            var attrVersion = _serviceTypeInfo.GetCustomAttribute<VersionAttribute>()?.Version;
            if (_useOnlyAttributes)
            {
                version = attrVersion;
            }
            else
            {

                version = version ?? attrVersion ?? Version.Parse("1.0");
            }
            if(version == null) throw new ServiceInterfaceException($"Cannot determinate version of {typeof(T)}");

            var serviceName = _serviceTypeInfo.GetCustomAttribute<ServiceNameAttribute>()?.Name;
            if (!_useOnlyAttributes)
                serviceName = serviceName ?? typeof(T).Name;
            if (serviceName == null)
                throw new ServiceInterfaceException($"Cannot determine service name of {typeof(T)}");



            var endpoints = typeof(T).GetProperties().Select(GenerateEndpoint)
                .ToDictionary(p => p.Item1, p => p.Item2);
            


            return new ServiceSchema
            {
                Version = version,
                Title = typeof(T).Name,
                Name = serviceName,
                AdditionalData = _extensions.Select(p => p.ExtendService(typeof(T))).Where(p => p != null).ToDictionary(p => p.Name, p => p.Value),
                Endpoints = endpoints
            };
        }

        private (string, EndpointSchema) GenerateEndpoint(PropertyInfo property)
        {
            var endpointName = property.GetCustomAttribute<EndpointNameAttribute>()?.Name;
            if (! _useOnlyAttributes)
                endpointName = endpointName ?? property.Name;
            if(endpointName == null)
                throw new ServiceInterfaceException($"Unknown endpoint name '{property.Name}' of {typeof(T)}");
            var propertyTypeInfo = property.PropertyType.GetTypeInfo();

            if (!propertyTypeInfo.IsGenericType)
                throw new ServiceInterfaceException($"Unknown property type '{property.Name}' of {typeof(T)}");
            var genericType = propertyTypeInfo.GetGenericTypeDefinition();
            if (genericType == typeof(IEventRune<>))
            {
                var contractSchema = GenerateContract(propertyTypeInfo.GenericTypeArguments[0]);
                return (endpointName, new EventEndpointSchema
                {
                    Title = property.Name,
                    Event = contractSchema,
                    AdditionalData = _extensions.Select(p => p.ExtendEndpoint(property)).Where(p => p != null)
                        .ToDictionary(p => p.Name, p => p.Value)
                });
            }
            if (genericType == typeof(ICommandRune<>))
            {
                return (endpointName, new CommandEndpointSchema
                {
                    Title = property.Name,
                    Command = GenerateContract(propertyTypeInfo.GenericTypeArguments[0]),
                    AdditionalData = _extensions.Select(p => p.ExtendEndpoint(property)).Where(p => p != null)
                        .ToDictionary(p => p.Name, p => p.Value)
                });
            }
            if (genericType == typeof(ICallableRune<,>))
            {
                return (endpointName, new CallableEndpointSchema
                {
                    Title = property.Name,
                    AdditionalData = _extensions.Select(p => p.ExtendEndpoint(property)).Where(p => p != null)
                        .ToDictionary(p => p.Name, p => p.Value),
                    Request = GenerateContract(propertyTypeInfo.GenericTypeArguments[0]),
                    Response = GenerateContract(propertyTypeInfo.GenericTypeArguments[1])
                });
            }
            throw new ServiceInterfaceException($"Unknown property type '{property.Name}' of {typeof(T)}");
        }

        private ContractTypeSchema GenerateContract(Type contractType)
        {
            var contractName = contractType.GetTypeInfo().GetCustomAttribute<ContractNameAttribute>()?.Name;
            if (!_useOnlyAttributes)
                contractName = contractName ?? contractType.Name;
            if (contractName == null)
                throw new ServiceInterfaceException($"Unknown contract type for '{contractType}");
            var contractJSchema = JObject.Parse(JsonSchema4.FromTypeAsync(contractType).Result.ToJson());
            var contractSchema = new ContractTypeSchema
            {
                Name = contractName,
                Title = contractType.Name,
                AdditionalData = _extensions.Select(p => p.ExtendContract(contractType)).Where(p => p != null)
                    .ToDictionary(p => p.Name, p => p.Value),
                Schema = contractJSchema
            };
            return contractSchema;
        }
    }
}
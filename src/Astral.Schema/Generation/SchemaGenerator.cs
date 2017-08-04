using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using Astral.Schema.Exceptions;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using NJsonSchema.Generation;

namespace Astral.Schema.Generation
{
    public class SchemaGenerator<T>
    {
        private readonly ISchemaGeneratorExtension[] _extensions;
        
        private readonly TypeInfo _serviceTypeInfo;

        public SchemaGenerator(params ISchemaGeneratorExtension[] extensions)
        {
            if(!typeof(T).GetTypeInfo().IsInterface) throw new ArgumentException($"Shema type must be interface");
            _extensions = extensions ?? new ISchemaGeneratorExtension[0];
            
            _serviceTypeInfo = typeof(T).GetTypeInfo();
        }

        public ServiceSchema Generate(SchemaGenerationOptions options = null)
        {
            options = options ?? new SchemaGenerationOptions();
            var serviceAttr = _serviceTypeInfo.GetCustomAttribute<ServiceAttribute>();
            if(serviceAttr == null)
                throw new ServiceInterfaceException($"Invalid service specification {typeof(T)} - no {nameof(ServiceAttribute)} specified");

            var serviceName = serviceAttr.Name ?? options.MemberNameToSchemaName?.Invoke(typeof(T).Name, true);
            if (serviceName == null)
                throw new ServiceInterfaceException($"Cannot determine service name of {typeof(T)}");

            var endpoints = new Dictionary<string, EndpointSchema>();
            var contracts = new List<Type>();
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                var (name, schema, types) = GenerateEndpoint(propertyInfo, options);
                endpoints.Add(name, schema);
                contracts.AddRange(types);
            }
            var containerGenerator = new ContainerClassBuilder(contracts.Distinct(),
                $"{typeof(T).Namespace}.{typeof(T).Name}Container");
            var containerType = containerGenerator.GenerateContainerType();

            var jsonSchema = JsonSchema4.FromTypeAsync(containerType, new JsonSchemaGeneratorSettings
            {
                SchemaProcessors = { new CustomSchemaProcessor() },
                
            }).Result;
            var containerJson = jsonSchema.ToJson();
            var containerJSchema = JObject.Parse(containerJson);

            return new ServiceSchema
            {
                Version = serviceAttr.Version,
                Title = typeof(T).Name,
                Transports = GetTransports(_serviceTypeInfo),
                Name = serviceName,
                AdditionalData = _extensions.Select(p => p.ExtendService(typeof(T))).Where(p => p != null).ToDictionary(p => p.Name, p => p.Value),
                Endpoints = endpoints,
                Contracts = containerJSchema
            };
        }

        private (string, EndpointSchema, IEnumerable<Type>) GenerateEndpoint(PropertyInfo property, SchemaGenerationOptions options)
        {
            var endpointAttr = property.GetCustomAttribute<EndpointAttribute>();
            var endpointName = endpointAttr?.Name ?? options.MemberNameToSchemaName?.Invoke(property.Name, false);
            if(endpointName == null)
                throw new ServiceInterfaceException($"Unknown endpoint name '{property.Name}' of {typeof(T)}");
            var propertyTypeInfo = property.PropertyType.GetTypeInfo();

            if (!propertyTypeInfo.IsGenericType)
                throw new ServiceInterfaceException($"Unknown property type '{property.Name}' of {typeof(T)}");
            var genericType = propertyTypeInfo.GetGenericTypeDefinition();
            if (genericType == typeof(IEvent<>))
            {
                var (type, schema) = GenerateContract(propertyTypeInfo.GenericTypeArguments[0], options);
                var types = type != null ? new[] { type } : new Type[0];
                return (endpointName, new EventEndpointSchema
                {
                    Title = property.Name,
                    Event = schema,
                    AdditionalData = _extensions.Select(p => p.ExtendEndpoint(property)).Where(p => p != null)
                        .ToDictionary(p => p.Name, p => p.Value),
                    Transports = GetTransports(property)
                }, types);
            }
            if (genericType == typeof(ICommand<>))
            {
                var (type, schema) = GenerateContract(propertyTypeInfo.GenericTypeArguments[0], options);
                var types = type != null ? new[] {type} : new Type[0];
                return (endpointName, new CommandEndpointSchema
                {
                    Title = property.Name,
                    Command = schema,
                    AdditionalData = _extensions.Select(p => p.ExtendEndpoint(property)).Where(p => p != null)
                        .ToDictionary(p => p.Name, p => p.Value),
                    Transports = GetTransports(property)
                }, types);
            }
            if (genericType == typeof(ICallable<,>))
            {
                var types = new List<Type>();
                var (intype, inschema) = GenerateContract(propertyTypeInfo.GenericTypeArguments[0], options);
                if(intype != null) types.Add(intype);
                var (outtype, outschema) = GenerateContract(propertyTypeInfo.GenericTypeArguments[0], options);
                if (outtype != null) types.Add(outtype);
                return (endpointName, new CallableEndpointSchema
                {
                    Title = property.Name,
                    AdditionalData = _extensions.Select(p => p.ExtendEndpoint(property)).Where(p => p != null)
                        .ToDictionary(p => p.Name, p => p.Value),
                    Request = inschema,
                    Response = outschema,
                    Transports = GetTransports(property)
                }, types);
            }
            throw new ServiceInterfaceException($"Unknown property type '{property.Name}' of {typeof(T)}");
        }

        private (Type, ContractTypeSchema) GenerateContract(Type contractType, SchemaGenerationOptions options)
        {
            if (AstralSchema.PrimitiveCodesByType.ContainsKey(contractType))
                return (null, new PrimitiveTypeSchema {Code = AstralSchema.PrimitiveCodesByType[contractType]});
            
            var typeInfo = contractType.GetTypeInfo();
            if (typeInfo.IsArray)
            {
                var elementType = typeInfo.GetElementType();
                var (elType, elementSchema) = GenerateContract(elementType, options);
                return (elType, new ArrayTypeSchema
                {
                    Title = $"{elementType.Name}[]",
                    Element = elementSchema
                });
            }
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var elementType = typeInfo.GenericTypeArguments[0];
                var (elType, elementSchema) = GenerateContract(elementType, options);
                return (elType, new ArrayTypeSchema
                {
                    Title = $"IEnumerable<{elementType.Name}>",
                    Element = elementSchema
                });
            }

            var knownTypeAttributes = typeInfo.GetCustomAttributes<KnownTypeAttribute>().ToArray();
            if(typeInfo.IsAbstract && knownTypeAttributes.Length == 0)
                throw new ServiceInterfaceException($"Contract {contractType} is abstract, but not have KnownTypeAttribute");
            if(knownTypeAttributes.Length == 0)
                return GenerateObjectContract(contractType, options);
            var types = new List<Type>();
            foreach (var knownTypeAttribute in knownTypeAttributes)
            {
                if (knownTypeAttribute.MethodName != null)
                {
                    var method = contractType.GetMethod(knownTypeAttribute.MethodName);
                    var subTypes = (IEnumerable<Type>)  method.Invoke(null, new object[0]);
                    types.AddRange(subTypes);
                }
                else
                    types.Add(knownTypeAttribute.Type);
            }
            if(!typeInfo.IsAbstract)
                types.Add(contractType);
            return (contractType, new HierarchyTypeSchema
            {
                RootTypeReference = contractType.Name,
                Title = contractType.Name,
                Members = types.Distinct().Select(p => GenerateObjectContract(p, options).Item2).ToArray()
            });
        }

        private (Type,ObjectTypeSchema) GenerateObjectContract(Type contractType, SchemaGenerationOptions options)
        {
            var contractAttr = contractType.GetTypeInfo().GetCustomAttribute<ContractAttribute>();
            if (contractAttr == null)
                throw new ServiceInterfaceException(
                    $"Invalid astral contract '{contractType}' -  {nameof(ContractAttribute)} not found");
            var contractName = contractAttr.Name ?? options.MemberNameToSchemaName?.Invoke(contractType.Name, false);
            if (contractName == null)
                throw new ServiceInterfaceException($"Unknown contract name for '{contractType}");


            var contractSchema = new ObjectTypeSchema
            {
                Name = contractName,
                Title = contractType.Name,
                Version = contractAttr.Version,
                AdditionalData = _extensions.Select(p => p.ExtendObjectContract(contractType)).Where(p => p != null)
                    .ToDictionary(p => p.Name, p => p.Value),
                TypeReference = contractType.Name
            };
            return (contractType, contractSchema);
        }

        private static Dictionary<TransportType, string> GetTransports(MemberInfo typeInfo)
        {
            var transports = new Dictionary<TransportType, string>();
            foreach (var transport in typeInfo.GetCustomAttributes<TransportAttribute>())
            {
                transports[transport.Type] = transport.Code;
            }
            return transports;
        }
    }
}
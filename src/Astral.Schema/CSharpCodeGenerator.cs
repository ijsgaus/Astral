﻿using System;
using System.Collections.Generic;
using System.Linq;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;

namespace Astral.Schema
{
    public class CSharpCodeGenerator
    {
        private readonly ServiceSchema _schema;
        private readonly CSharpCodeGenerationOptions _options;
        private readonly ICSharpCodeGeneratorExtension[] _extensions;

        public CSharpCodeGenerator(ServiceSchema schema, CSharpCodeGenerationOptions options, params ICSharpCodeGeneratorExtension[] extensions)
        {
            _schema = schema;
            _options = options;
            _extensions = extensions;
        }

        


        public string GenerateInterface()
        {
            var writer = new IndentWriter();
            
            writer.WriteLine($"namespace {_options.Namespace}");
            writer.WriteLine("{");
            using (writer.Indent())
            {
                writer.WriteLine("using System;");
                writer.WriteLine("using Astral;");
                _extensions.Iter(p => p.WriteNamespaces(writer, _schema));
                writer.WriteLine();
                writer.WriteLine($"[Service(\"{_schema.Version}\", \"{_schema.Name}\")]");
                WriteTransports(_schema, writer);
                _extensions.Iter(p => p.WriteServiceAttributes(writer, _schema));
                writer.WriteLine($"public interface {_options.InterfaceName ?? _schema.Title}");
                writer.WriteLine("{");
                using (writer.Indent())
                {
                    foreach (var endpoint in _schema.Endpoints)
                    {
                        if(_options.Endpoints.IsSkip(endpoint.Value.Title)) continue;
                        WriteTransports(endpoint.Value, writer);
                        writer.WriteLine($"[Endpoint(\"{endpoint.Key}\")]");
                        _extensions.Iter(p => p.WriteEndpointAttributes(writer, endpoint.Value));

                        switch (endpoint.Value)
                        {
                            case EventEndpointSchema ees:
                                
                                writer.WriteLine($"IEvent<{GetTypeNameByContract(ees.Event)}> {ees.Title} " + "{ get; }");
                                break;
                            case CommandEndpointSchema ces:
                                writer.WriteLine($"ICommand<{GetTypeNameByContract(ces.Command)}> {ces.Title} " + "{ get; }");
                                break;
                            case CallableEndpointSchema cls:
                                writer.WriteLine($"ICallable<{GetTypeNameByContract(cls.Request)}, {GetTypeNameByContract(cls.Response)}> {cls.Title} " + "{ get; }");
                                break;
                            default:
                                throw new InvalidOperationException($"Unknown endpoint type {endpoint.Value.GetType()}");
                        }
                    }
                }
                writer.WriteLine("}");

            }
            writer.WriteLine("}");
            return writer.ToString();
        }

        private void WriteTransports(GatePartSchema gate, IndentWriter writer)
        {
            if (_schema.Transports != null)
                foreach (var transport in gate.Transports)
                {
                    writer.WriteLine($"[Transport(TransportType.{transport.Key}, \"{transport.Value}\")");
                }
        }

        private string GetTypeNameByContract(ContractTypeSchema schema)
        {
            switch (schema)
            {
                case WellKnownTypeSchema wts:
                    if (wts.Code == WellKnownTypes.UnitTypeCode)
                        return _options.UnitType.FullName;
                    return WellKnownTypes.GetCSharpTypeByCode(wts.Code);
                case ObjectTypeSchema ots:
                    return ots.TypeReference;
                case ArrayTypeSchema ats:
                    if (ats.Title.StartsWith("IEnumerable"))
                        return $"{typeof(IEnumerable<>).Namespace}.IEnumerable<{GetTypeNameByContract(ats.Element)}>";
                    return $"{GetTypeNameByContract(ats.Element)}[]";
               default:
                   throw new ArgumentOutOfRangeException($"Unknow contract type schema {schema.GetType()}");
            }
        }

        public string GenerateContracts()
        {
            var jsonSchema = JsonSchema4.FromJsonAsync(_schema.Contracts.ToString()).Result;

            foreach (var @class in _options.GeneratedProperties)
            {
                var jsonClass = jsonSchema.Definitions[@class.Key];
                foreach (var oproperty in jsonClass.Properties.ToList())
                {
                    if (@class.Value.IsSkip(oproperty.Key))
                       jsonClass.Properties.Remove(oproperty.Key);
                }
            }

            var generator = new CSharpGenerator(jsonSchema, new CSharpGeneratorSettings
            {
                Namespace = _options.Namespace,
                ClassStyle = CSharpClassStyle.Poco,
                DateTimeType = _options.DateTimeType.Name,
                DateType = _options.DateType.Name,
                ExcludedTypeNames = new [] { "Container" }.Union(_options.ExcludedTypes).ToArray(),
                TemplateFactory = new CustomTemplateFactory(jsonSchema)
            });
            return generator.GenerateFile();
        }
    }
}
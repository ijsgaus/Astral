using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using System.Linq;
using System.Text;

namespace Astral.Schema.Generation
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
                writer.WriteLine("using Astral.Runes;");
                _extensions.Iter(p => p.WriteNamespaces(writer, _schema));
                writer.WriteLine();

                writer.WriteLine($"[Service(\"{_schema.Version}\", \"{_schema.Name}\")]");
                if(_schema.Transports != null)
                    foreach (var transport in _schema.Transports)
                    {
                        writer.WriteLine($"[Transport(TransportType.{transport.Key}, \"{transport.Value}\")");
                    }
                _extensions.Iter(p => p.WriteServiceAttributes(writer, _schema));
                writer.WriteLine($"public interface {_options.InterfaceName ?? _schema.Title}");
                writer.WriteLine("{");
                using (writer.Indent())
                {
                    
                }
                writer.WriteLine("}");

            }
            writer.WriteLine("}");
            return writer.ToString();
        }
        
        public string GenerateContracts()
        {
            var jsonSchema = JsonSchema4.FromJsonAsync(_schema.Contracts.ToString()).Result;
            var generator = new CSharpGenerator(jsonSchema, new CSharpGeneratorSettings
            {
                Namespace = _options.Namespace,
                ClassStyle = CSharpClassStyle.Poco,
                DateTimeType = _options.DateTimeType.Name,
                DateType = _options.DateType.Name,
                ExcludedTypeNames = new [] { "Container" }.Union(_options.ExcludeTypes).ToArray(),
                TemplateFactory = new CustomTemplateFactory(jsonSchema)
            });
            return generator.GenerateFile();
        }
    }
}
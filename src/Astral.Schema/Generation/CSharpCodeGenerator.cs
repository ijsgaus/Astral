using NJsonSchema;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp;
using NJsonSchema.CodeGeneration.CSharp.Models;

namespace Astral.Schema.Generation
{
    public class CSharpCodeGenerator
    {
        private readonly ServiceSchema _schema;

        public CSharpCodeGenerator(ServiceSchema schema)
        {
            _schema = schema;
        }

        public string GenerateContracts(string nameSpace)
        {
            var jsonSchema = JsonSchema4.FromJsonAsync(_schema.Contracts.ToString()).Result;
            var generator = new CSharpGenerator(jsonSchema, new CSharpGeneratorSettings
            {
                Namespace = nameSpace,
                ClassStyle = CSharpClassStyle.Poco,
                DateTimeType = "DateTimeOffset",
                DateType = "DateTimeOffset",
                ExcludedTypeNames = new [] { "Container" },
                TemplateFactory = new CustomTemplateFactory(jsonSchema)
            });
            return generator.GenerateFile();
        }

        private class CustomTemplateFactory : ITemplateFactory
        {
            private readonly JsonSchema4 _schema;
            private static readonly ITemplateFactory Default  = new DefaultTemplateFactory();

            public CustomTemplateFactory(JsonSchema4 schema)
            {
                _schema = schema;
            }

            public ITemplate CreateTemplate(string package, string template, object model)
            {
                if (model is ClassTemplateModel ctm)
                {
                    var modelSchema = _schema.Definitions[ctm.Class];

                }
                return Default.CreateTemplate(package, template, model);
            }
        }
    }
}
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;

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
    }
}
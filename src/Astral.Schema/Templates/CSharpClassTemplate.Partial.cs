using NJsonSchema;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp.Models;

namespace Astral.Schema.Templates
{
    internal partial class CSharpClassTemplate : ITemplate
    {

        public CSharpClassTemplate(ClassTemplateModel model, CSharpClassTemplateOptions options)
        {
            Model = model;
            Options = options;
        }

        public ClassTemplateModel Model { get; }
        public CSharpClassTemplateOptions Options { get; }

        public string Render()
        {
            return ConversionUtilities.TrimWhiteSpaces(TransformText());
        }

    }
}
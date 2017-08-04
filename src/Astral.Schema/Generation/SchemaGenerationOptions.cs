using System;

namespace Astral.Schema.Generation
{
    public class SchemaGenerationOptions
    {
        public Func<string, bool, string> MemberNameToSchemaName { get; set; } = null;
    }
}
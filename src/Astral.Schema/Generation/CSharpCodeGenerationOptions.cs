using System;

namespace Astral.Schema.Generation
{
    public class CSharpCodeGenerationOptions
    {
        public CSharpCodeGenerationOptions(string @namespace)
        {
            Namespace = @namespace;
        }

        public string Namespace { get; }
        public Type DateTimeType { get; set; } = typeof(DateTimeOffset);
        public Type DateType { get; set; } = typeof(DateTimeOffset);
        public string[] ExcludeTypes { get; set; } = new string[0];
        public Type UnitType { get; set; } = typeof(ValueTuple);
        public string InterfaceName { get; set; }
    }
}
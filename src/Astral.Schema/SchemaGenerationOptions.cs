using System;

namespace Astral.Schema
{
    public class SchemaGenerationOptions
    {
        public Func<string, bool, string> MemberNameToSchemaName { get; set; } = null;
        public Type UnitType { get; set; } = typeof(ValueTuple);
        public Type[] OtherUnitTypes { get; set; } = new Type[0];
    }
}
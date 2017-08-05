using System;

namespace Astral
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class SchemaAttribute : Attribute
    {
        public SchemaAttribute(string schema)
        {
            Schema = schema;
        }

        public string Schema { get; }
    }
}
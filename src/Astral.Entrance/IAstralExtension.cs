using System;
using Newtonsoft.Json.Linq;

namespace Astral
{
    public interface IAstralExtension
    {
        string Name { get; }
    }

    public interface IAstralSchemaExtension : IAstralExtension
    {
        Attribute ToAttribute(JProperty property);
    }
}
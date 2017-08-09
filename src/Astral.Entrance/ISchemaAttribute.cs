using Astral.Symbols;
using Newtonsoft.Json.Linq;

namespace Astral
{
    public interface ISchemaAttribute
    {
        string ExtensionName { get; }
        JProperty ToProperty();
        ISymbol ToSymbol();
    }
    
    
}
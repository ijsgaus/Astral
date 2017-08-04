using System;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Astral.Schema.Exceptions
{
    public interface ISchemaGeneratorExtension
    {
        JProperty ExtendService(Type serviceType);
        JProperty ExtendEndpoint(PropertyInfo propertyInfo);
        JProperty ExtendObjectContract(Type contractType);
    }
}
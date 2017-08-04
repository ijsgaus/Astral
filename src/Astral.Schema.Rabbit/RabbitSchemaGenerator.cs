using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Astral.Runes.Rabbit;
using Astral.Schema.Exceptions;
using Newtonsoft.Json.Linq;
using Astral.Tools;
using static Astral.Schema.Rabbit.SchemaNames;


namespace Astral.Schema.Rabbit
{
    public class RabbitSchemaGenerator : ISchemaGeneratorExtension
    {
        public JProperty ExtendService(Type serviceType)
        {
            var typeInfo = serviceType.GetTypeInfo();
            var properties = new List<JProperty>();
            ProcessExchangeAttributes(typeInfo, properties, false);
            
            return 
                properties.Count > 0
                ? new JProperty(RabbitMqSection, new JObject(properties.Cast<object>().ToArray()))
                : null;
        }

        public JProperty ExtendEndpoint(PropertyInfo propertyInfo)
        {
            var properties = new List<JProperty>();
            ProcessExchangeAttributes(propertyInfo, properties, true);
            propertyInfo.GetCustomAttribute<RoutingKeyAttribute>()
                .NotNullMap(p => new JProperty(PropRoutingKey, p.Key))
                .NotNullDo(p => properties.Add(p));

            return 
                properties.Count > 0
                ? new JProperty(RabbitMqSection, new JObject(properties.Cast<object>().ToArray()))
                : null;
        }



        public JProperty ExtendObjectContract(Type contractType)
        {
            return null;
        }

        private static void ProcessExchangeAttributes(MemberInfo typeInfo, List<JProperty> properties, bool withoutType)
        {
            var exchange = typeInfo.GetCustomAttribute<ExchangeAttribute>().NotNullMap(p => new[]
                {new JProperty(PropExchangeName, p.Name), new JProperty(PropExchangeType, p.Type.ToJsonString())});
            if (exchange == null)
            {
                if(!withoutType)
                    typeInfo.GetCustomAttribute<ExchangeTypeAttribute>()
                        .NotNullMap(p => new JProperty(PropExchangeType, p.Type.ToJsonString()))
                        .NotNullDo(properties.Add);
            }
            else
                properties.AddRange(exchange);

            var respExchange = typeInfo.GetCustomAttribute<ResponseExchangeAttribute>().NotNullMap(p => new[]
            {
                new JProperty(PropResponseExchangeName, p.Name), new JProperty(PropResponseExchangeType, p.Type.ToJsonString())
            });
            if (respExchange == null)
            {
                if (!withoutType)
                    typeInfo.GetCustomAttribute<ResponseExchangeTypeAttribute>()
                    .NotNullMap(p => new JProperty(PropResponseExchangeType, p.Type.ToJsonString()))
                    .NotNullDo(properties.Add);
            }
            else
                properties.AddRange(respExchange);
        }

        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Astral.Runes.Rabbit;
using Astral.Schema.Exceptions;
using Newtonsoft.Json.Linq;
using Astral.Tools;


namespace Astral.Schema.Rabbit
{
    public class RabbitSchemaGenerator : ISchemaGeneratorExtension
    {
        public JProperty ExtendService(Type serviceType)
        {
            var typeInfo = serviceType.GetTypeInfo();
            var properties = new List<JProperty>();
            ProcessExchangeAttributes(typeInfo, properties, false);

            return new JProperty("rabbitmq", new JObject(properties.Cast<object>().ToArray()));
        }

        public JProperty ExtendEndpoint(PropertyInfo propertyInfo)
        {
            var properties = new List<JProperty>();
            ProcessExchangeAttributes(propertyInfo, properties, true);
            propertyInfo.GetCustomAttribute<RoutingKeyAttribute>().NotNullMap(p => new JProperty("routingKey", p.Key)).NotNullDo(p => properties.Add(p));

            return new JProperty("rabbitmq", new JObject(properties.Cast<object>().ToArray()));
        }

        public JProperty ExtendContract(Type contractType)
        {
            return null;
        }

        private static void ProcessExchangeAttributes(MemberInfo typeInfo, List<JProperty> properties, bool withoutType)
        {
            var exchange = typeInfo.GetCustomAttribute<ExchangeAttribute>().NotNullMap(p => new[]
                {new JProperty("exchange", p.Name), new JProperty("exchangeType", p.Type.ToString().ToLower())});
            if (exchange == null)
            {
                if(!withoutType)
                    typeInfo.GetCustomAttribute<ExchangeTypeAttribute>()
                        .NotNullMap(p => new JProperty("exchangeType", p.Type.ToString().ToLower()))
                        .NotNullDo(properties.Add);
            }
            else
                properties.AddRange(exchange);

            var respExchange = typeInfo.GetCustomAttribute<ResponseExchangeAttribute>().NotNullMap(p => new[]
            {
                new JProperty("responseExchange", p.Name), new JProperty("responseExchangeType", p.Type.ToString().ToLower())
            });
            if (respExchange == null)
            {
                if (!withoutType)
                    typeInfo.GetCustomAttribute<ResponseExchangeTypeAttribute>()
                    .NotNullMap(p => new JProperty("responseExchangeType", p.Type.ToString().ToLower()))
                    .NotNullDo(properties.Add);
            }
            else
                properties.AddRange(respExchange);
        }

        
    }
}
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading;
using Astral.Schema;
using Astral.Transports;
using LanguageExt;

namespace Astral.Configuraiton
{
    public class GateConfig : DisposableBag
    {
        private readonly SchemaGenerationOptions _schemaGenerationOptions;
        private readonly TransportConfig _transportConfig;

        private readonly ConcurrentDictionary<Type, ServiceSchema> _schemaCache = new ConcurrentDictionary<Type, ServiceSchema>();
        private readonly ConcurrentDictionary<Type, ServiceConfig> _serviceConfigCache  = new ConcurrentDictionary<Type, ServiceConfig>();

        internal GateConfig(
            SchemaGenerationOptions schemaGenerationOptions,
            TransportConfig transportConfig)
        {
            _schemaGenerationOptions = schemaGenerationOptions;
            _transportConfig = transportConfig;
            Disposables.Add(transportConfig);
        }

        public ServiceConfig<T> Service<T>()
        {
            throw new NotImplementedException();
        }

        private ServiceSchema GetServiceSchema<T>()
        {
            return _schemaCache.GetOrAdd(typeof(T), _ =>
            {
                var typeInfo = typeof(T).GetTypeInfo();
                var attr = typeInfo.GetCustomAttribute<ServiceAttribute>();
                if (attr == null)
                    throw new ArgumentException($"Invalid service type {typeof(T)} - not ServiceAttribute found");
                var schemaAttr = typeInfo.GetCustomAttribute<SchemaAttribute>();
                var schema = schemaAttr == null ? new SchemaGenerator<T>().Generate(_schemaGenerationOptions) : ServiceSchema.FromString(schemaAttr.Schema);
                return schema;
            });
        }
        
        


        internal IRpcTransport GetRpcPorter(IServiceProvider provider, string code = null)
        {
            return _transportConfig.GetRpcPorter(provider, code);
        }

        internal IQueueTransport GetQueuePorter(IServiceProvider provider, string code = null)
        {
            return _transportConfig.GetQueuePorter(provider, code);
        }
    }

    
}
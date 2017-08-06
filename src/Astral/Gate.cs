using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading;
using Astral.Configuraiton;
using Astral.Gates;
using Astral.Schema;
using Astral.Transports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Astral
{
    public class Gate : DisposableBag
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly GateConfig _config;

        internal Gate(ILoggerFactory loggerFactory, GateConfig config)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<Gate>();
            _config = config;
            Disposables.Add(config);
        }

        public IServiceGate<T> Service<T>()
        {
            CheckDisposed();
            var typeInfo = typeof(T).GetTypeInfo();
            if(!typeInfo.IsInterface) throw new ArgumentException($"Invalid service type {typeof(T)} - must be interface");
            
            return new ServiceGate<T>(_config, _config.GetServiceSchema<T>())
                .WithSide(() => _logger.LogTrace("Service gate for service {service}", typeof(T)));
        }


        private static readonly ConcurrentDictionary<string, Type> RpcPorterTypes = new ConcurrentDictionary<string, Type>();
        private static readonly ConcurrentDictionary<string, Type> QueuePorterTypes = new ConcurrentDictionary<string, Type>();
        private readonly ILogger<Gate> _logger;

        public static void RegisterPorterType<T>(string porterCode, bool asDefault = true)
            where T : ITransport
        {
            var success = false;
            if (typeof(IRpcTransport).IsAssignableFrom(typeof(T)))
            {
                success = true;
                RpcPorterTypes.AddOrUpdate("", _ => typeof(T), (s, t) => asDefault ? typeof(T) : t);
                RpcPorterTypes.AddOrUpdate(porterCode, _ => typeof(T), (s, t) => typeof(T));
            }
            if (typeof(IQueueTransport).IsAssignableFrom(typeof(T)))
            {
                success = true;
                QueuePorterTypes.AddOrUpdate("", _ => typeof(T), (s, t) => asDefault ? typeof(T) : t);
                QueuePorterTypes.AddOrUpdate(porterCode, _ => typeof(T), (s, t) => typeof(T));
            }
            if(!success) throw new ArgumentOutOfRangeException($"Unknown trnsport subtype for {typeof(T)}");
        }

        internal static IRpcTransport GetRpcPorter(string code, IServiceProvider serviceProvider)
        {
            if (RpcPorterTypes.TryGetValue(code, out var type))
                return (IRpcTransport) serviceProvider.GetRequiredService(type);
            var codeName = code == "" ? "default" : $"'{code}'";
            throw new InvalidOperationException($"Cannot find {codeName} rpc transport ");
        }

        internal static IQueueTransport GetQueuePorter(string code, IServiceProvider serviceProvider)
        {
            if (RpcPorterTypes.TryGetValue(code, out var type))
                return (IQueueTransport)serviceProvider.GetRequiredService(type);
            var codeName = code == "" ? "default" : $"'{code}'";
            throw new InvalidOperationException($"Cannot find {codeName} queue transport ");
        }

        internal static IEnumerable<Type> GetAllPorterTypes()
            => RpcPorterTypes.Values.Union(QueuePorterTypes.Values).Distinct();
    }
}
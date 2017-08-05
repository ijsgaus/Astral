using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading;
using Astral.Porters;
using Microsoft.Extensions.DependencyInjection;

namespace Astral.Configuraiton
{
    public class GateBuilder
    {
        private static readonly ConcurrentDictionary<string, Type> RpcPorterTypes = new ConcurrentDictionary<string, Type>();
        private static readonly ConcurrentDictionary<string, Type> QueuePorterTypes = new ConcurrentDictionary<string, Type>();

        private readonly ConcurrentDictionary<string, IRpcPorter> _rpcPorters = new ConcurrentDictionary<string, IRpcPorter>();
        private readonly ConcurrentDictionary<string, IQueuePorter> _queuePorters = new ConcurrentDictionary<string, IQueuePorter>();

        private readonly GateConfig _config = new GateConfig();

        public static void RegisterPorterType<T>(string porterCode, bool asDefault = true)
            where T : IPorter
        {
            var success = false;
            if (typeof(IRpcPorter).IsAssignableFrom(typeof(T)))
            {
                success = true;
                RpcPorterTypes.AddOrUpdate("", _ => typeof(T), (s, t) => asDefault ? typeof(T) : t);
                RpcPorterTypes.AddOrUpdate(porterCode, _ => typeof(T), (s, t) => typeof(T));
            }
            if (typeof(IQueuePorter).IsAssignableFrom(typeof(T)))
            {
                success = true;
                QueuePorterTypes.AddOrUpdate("", _ => typeof(T), (s, t) => asDefault ? typeof(T) : t);
                QueuePorterTypes.AddOrUpdate(porterCode, _ => typeof(T), (s, t) => typeof(T));
            }
            if(!success) throw new ArgumentOutOfRangeException($"Unknown trnsport subtype for {typeof(T)}");
        }

        public void RegisterPorter<T>(string porterCode, T porter, PorterType? type = null, bool asDefault = true)
            where T : IPorter
            => _config.RegisterPorter(porterCode, porter, type, asDefault);

        internal static IRpcPorter GetRpcPorter(string code, IServiceProvider serviceProvider)
        {
            if (RpcPorterTypes.TryGetValue(code, out var type))
                return (IRpcPorter) serviceProvider.GetRequiredService(type);
            var codeName = code == "" ? "default" : $"'{code}'";
            throw new InvalidOperationException($"Cannot find {codeName} rpc transport ");
        }

        internal static IQueuePorter GetQueuePorter(string code, IServiceProvider serviceProvider)
        {
            if (RpcPorterTypes.TryGetValue(code, out var type))
                return (IQueuePorter)serviceProvider.GetRequiredService(type);
            var codeName = code == "" ? "default" : $"'{code}'";
            throw new InvalidOperationException($"Cannot find {codeName} queue transport ");
        }

        public Gate Build()
        {
            return new Gate(_config);
        }
    }
}
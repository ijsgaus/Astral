using System;
using System.Collections.Concurrent;
using Astral.Transports;

namespace Astral.Configuraiton
{
    internal class TransportConfig : DisposableBag
    {
        private readonly ConcurrentDictionary<string, IRpcTransport> _rpcPorters = new ConcurrentDictionary<string, IRpcTransport>();
        private readonly ConcurrentDictionary<string, IQueueTransport> _queuePorters = new ConcurrentDictionary<string, IQueueTransport>();

        internal void RegisterTransport<T>(string porterCode, T porter, TransportType? type = null, bool asDefault = true)
            where T : ITransport
        {
            CheckDisposed();
            var success = false;
            if (porter is IRpcTransport rpcPorter)
            {
                
                _rpcPorters.AddOrUpdate(porterCode, _ => rpcPorter, (_, o) =>
                {
                    if (o is IDisposable od)
                    {
                        Disposables.Remove(od);
                        od.Dispose();
                    }
                    return rpcPorter;
                });
                var rpcDefault = asDefault && (type == null || type.Value == TransportType.Rpc);
                _rpcPorters.AddOrUpdate("", _ => rpcPorter, (_, o) => rpcDefault ? rpcPorter : o);
                if (porter is IDisposable d) Disposables.Add(d);
                success = true;
            }

            if (porter is IQueueTransport queuePorter)
            {
                
                _queuePorters.AddOrUpdate(porterCode, _ => queuePorter, (_, o) =>
                {
                    if (o is IDisposable od)
                    {
                        Disposables.Remove(od);
                        od.Dispose();
                    }
                    return queuePorter;
                });
                var queueDefault = asDefault && (type == null || type.Value == TransportType.Queue);
                _queuePorters.AddOrUpdate("", _ => queuePorter, (_, o) => queueDefault ? queuePorter : o);
                if (!success && porter is IDisposable d) Disposables.Add(d);
                success = true;
            }
            if (!success) throw new ArgumentOutOfRangeException($"Unknown trnsport subtype for {typeof(T)}");
        }

        internal IRpcTransport GetRpcPorter(IServiceProvider provider, string code = null)
        {
            CheckDisposed();
            code = code ?? "";
            if (_rpcPorters.TryGetValue(code, out var porter)) return porter;
            if (provider != null)
                return Gate.GetRpcPorter(code, provider);
            var codeName = code == "" ? "default" : $"'{code}'";
            throw new InvalidOperationException($"Cannot find {codeName} rpc transport ");
        }

        internal IQueueTransport GetQueuePorter(IServiceProvider provider, string code = null)
        {
            CheckDisposed();
            code = code ?? "";
            if (_queuePorters.TryGetValue(code, out var porter)) return porter;
            if (provider != null)
                return Gate.GetQueuePorter(code, provider);
            var codeName = code == "" ? "default" : $"'{code}'";
            throw new InvalidOperationException($"Cannot find {codeName} rpc transport ");
        }
    }
}
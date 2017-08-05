using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Threading;
using Astral.Porters;

namespace Astral.Configuraiton
{
    public class GateConfig : IDisposable
    {
        public GateConfig()
        {
        }

        private readonly ConcurrentDictionary<string, IRpcPorter> _rpcPorters = new ConcurrentDictionary<string, IRpcPorter>();
        private readonly ConcurrentDictionary<string, IQueuePorter> _queuePorters = new ConcurrentDictionary<string, IQueuePorter>();

        private int _isDisposed = 0;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        internal void RegisterPorter<T>(string porterCode, T porter, PorterType? type = null, bool asDefault = true)
            where T : IPorter
        {
            CheckDisposed();
            var success = false;
            if (porter is IRpcPorter rpcPorter)
            {
                success = true;
                _rpcPorters.AddOrUpdate(porterCode, _ => rpcPorter, (_, o) =>
                {
                    if (o is IDisposable d) d.Dispose();
                    return rpcPorter;
                });
                var rpcDefault = asDefault && (type == null || type.Value == PorterType.Rpc);
                _rpcPorters.AddOrUpdate("", _ => rpcPorter, (_, o) => rpcDefault ? rpcPorter : o);
            }

            if (porter is IQueuePorter queuePorter)
            {
                success = true;
                _queuePorters.AddOrUpdate(porterCode, _ => queuePorter, (_, o) =>
                {
                    if (o is IDisposable d) d.Dispose();
                    return queuePorter;
                });
                var queueDefault = asDefault && (type == null || type.Value == PorterType.Queue);
                _queuePorters.AddOrUpdate("", _ => queuePorter, (_, o) => queueDefault ? queuePorter : o);
            }
            if (!success) throw new ArgumentOutOfRangeException($"Unknown trnsport subtype for {typeof(T)}");
        }

        internal IRpcPorter GetRpcPorter(IServiceProvider provider, string code = null)
        {
            code = code ?? "";
            if (_rpcPorters.TryGetValue(code, out var porter)) return porter;
            if (provider != null)
                return GateBuilder.GetRpcPorter(code, provider);
            var codeName = code == "" ? "default" : $"'{code}'";
            throw new InvalidOperationException($"Cannot find {codeName} rpc transport ");
        }

        internal IQueuePorter GetQueuePorter(IServiceProvider provider, string code = null)
        {
            code = code ?? "";
            if (_queuePorters.TryGetValue(code, out var porter)) return porter;
            if (provider != null)
                return GateBuilder.GetQueuePorter(code, provider);
            var codeName = code == "" ? "default" : $"'{code}'";
            throw new InvalidOperationException($"Cannot find {codeName} rpc transport ");
        }

        internal void Freeze()
        {
            _queuePorters.Iter(p =>
            {
                if (p.Value is IDisposable d) _disposable.Add(d);
            });
            _rpcPorters.Iter(p =>
            {
                if (p.Value is IDisposable d) _disposable.Add(d);
            });
        }

        private void CheckDisposed()
        {
            if (Interlocked.CompareExchange(ref _isDisposed, 0, 0) == 1)
                throw new ObjectDisposedException(GetType().Name);
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _isDisposed, 0, 1) == 1) return;
            _disposable.Dispose();
        }



        




    }
}
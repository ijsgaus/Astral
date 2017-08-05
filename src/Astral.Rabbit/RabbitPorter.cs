using System;
using System.Reactive.Disposables;
using System.Threading;
using Astral.Porters;
using RabbitLink;

namespace Astral.Rabbit
{
    public class RabbitPorter : IRpcPorter, IQueuePorter, IDisposable
    {
        private readonly Link _link;

        public RabbitPorter(RabbitMqConfig config)
        {
            _link = new Link(config.Url, cfg => config.Apply(cfg));
            _disposable.Add(_link);
        }

        private int _isDisposed = 0;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

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
﻿using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Threading;
using Astral.Configuraiton;
using Astral.Gates;
using Astral.Porters;

namespace Astral
{
    public class Gate : IDisposable
    {
        private readonly GateConfig _config;

        internal Gate(GateConfig config)
        {
            _config = config;
            _disposable.Add(config);
        }

        public IServiceGate<T> Service<T>()
        {
            CheckDisposed();
            throw new NotImplementedException();
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
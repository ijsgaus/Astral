using System;
using System.Reactive.Disposables;

namespace Astral
{
    public abstract class DisposableBag : IDisposable
    {
        protected CompositeDisposable Disposables { get; } = new CompositeDisposable();


        protected void CheckDisposed()
        {
            if (Disposables.IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        public bool IsDisposed => Disposables.IsDisposed;

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
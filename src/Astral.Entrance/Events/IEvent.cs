using System;
using System.Threading;
using System.Threading.Tasks;
using Astral.Data;

namespace Astral
{
    public interface IEvent<T> : ILoggable
    {
        Task PublishAsync(T message, CancellationToken token);

        (Guid, Action) PrepareDelivery<TStore>(TStore store, T message, TimeSpan ttl)
            where TStore : IDeliveryRepositoryProvider;

        IDisposable Subscribe(Func<IEventConsumeContext<T>, CancellationToken, Task> handler, EventConsumeOptions options);

    }
}
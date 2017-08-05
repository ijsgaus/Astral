using System;
using System.Threading;
using System.Threading.Tasks;
using Astral.Data;

namespace Astral
{
    public static class EventExtensions
    {
        public static Task PublishAsync<T>(this IEvent<T> @event, T message)
            => @event.PublishAsync(message, CancellationToken.None);

        public static void PublishAndForget<T>(this IEvent<T> @event, T message)
            => @event.PublishAsync(message, CancellationToken.None);

        public static Guid Deliver<TStore, T>(this IEvent<T> @event, TStore store, T message, TimeSpan ttl)
            where TStore : IDeliveryRepositoryProvider, IRegisterAfterCommit
        {
            var (deliveryId, starter) = @event.PrepareDelivery(store, message, ttl);
            store.RegisterAfterCommit(starter);
            return deliveryId;
        }

        public static Guid Deliver<TStore, T>(this IEvent<T> @event, TStore store, T message)
            where TStore : IDeliveryRepositoryProvider, IRegisterAfterCommit
            => @event.Deliver(store, message, Timeout.InfiniteTimeSpan);
    }
}
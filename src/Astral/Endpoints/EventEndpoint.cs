using System;
using System.Threading;
using System.Threading.Tasks;
using Astral.Configuraiton;
using Astral.Data;
using Astral.Schema;
using Astral.Transports;
using Microsoft.Extensions.Logging;

namespace Astral.Endpoints
{
    public class EventEndpoint<TEvent> : EndpointBase, IEvent<TEvent>
    {
        private readonly EventEndpointConfig<TEvent> _config;


        internal EventEndpoint(ILoggerFactory loggerFactory, 
            EventEndpointConfig<TEvent> config) : base(loggerFactory, config)
        {
            _config = config;
        }

        public Task PublishAsync(TEvent message, CancellationToken token)
        {
            var transport = _config.GetTransport<IQueueTransport>();
            throw new NotImplementedException();
        }

        public (Guid, Action) PrepareDelivery<TStore>(TStore store, TEvent message, TimeSpan ttl) where TStore : IDeliveryRepositoryProvider
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(Func<IEventConsumeContext<TEvent>, CancellationToken, Task> handler, EventConsumeOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
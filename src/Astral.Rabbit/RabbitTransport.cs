using System;
using System.Reactive.Disposables;
using System.Threading;
using Astral.Transports;
using RabbitLink;

namespace Astral.Rabbit
{
    public class RabbitTransport : DisposableBag, IRpcTransport, IQueueTransport
    {
        private readonly Link _link;

        public RabbitTransport(RabbitMqConfig config)
        {
            _link = new Link(config.Url, cfg => config.Apply(cfg));
            Disposables.Add(_link);
        }

        
    }
}
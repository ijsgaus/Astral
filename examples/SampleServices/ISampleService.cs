using Astral;
using Astral.Runes;
using Astral.Runes.Rabbit;

namespace SampleServices
{
    [Service("0.5", "sample.service")]
    [ExchangeType(BusExchangeType.Topic)]
    public interface ISampleService
    {
        [Endpoint("awesome.event")]
        [Exchange("topic.exchange", BusExchangeType.Fanout)]
        IEvent<SampleEvent> AwesomeEvent { get; }

        [Endpoint("order.change")]
        [RoutingKey("special.routing.key")]
        ICommand<SampleCommand> OrderChange { get; }

        [Endpoint("an.array")]
        IEvent<int[]> AnArray { get; }
    }
}
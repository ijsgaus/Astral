using Astral.Runes;
using Astral.Runes.Rabbit;

namespace SampleServices
{
    [Version("0.5")]
    [ServiceName("sample.service")]
    [ExchangeType(BusExchangeType.Topic)]
    public interface ISampleService
    {
        [EndpointName("awesome.event")]
        [Exchange("topic.exchange", BusExchangeType.Fanout)]
        IEventRune<SampleEvent> AwesomeEvent { get; }

        [EndpointName("order.change")]
        [RoutingKey("special.routing.key")]
        ICommandRune<SampleCommand> OrderChange { get; }
    }
}
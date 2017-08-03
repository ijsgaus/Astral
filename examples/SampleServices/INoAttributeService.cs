using Astral.Runes;

namespace SampleServices
{
    public interface INoAttributeService
    {
        IEventRune<SampleEvent> AwesomeEvent { get; }
        ICommandRune<SampleCommand> OrderChange { get; }
    }
}
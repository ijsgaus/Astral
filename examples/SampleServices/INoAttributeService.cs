using Astral;
using Astral.Runes;

namespace SampleServices
{
    public interface INoAttributeService
    {
        IEvent<SampleEvent> AwesomeEvent { get; }
        ICommand<SampleCommand> OrderChange { get; }
    }
}
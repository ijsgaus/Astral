using Astral.Runes;

namespace SampleServices
{
    [ContractName("sample.event")]
    public class SampleEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SampleEnum Order { get; set; }
    }
}
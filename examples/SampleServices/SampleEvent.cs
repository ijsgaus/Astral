using System;
using System.Runtime.Serialization;
using Astral;
using Astral.Runes;

namespace SampleServices
{
    [Contract("0.5", "sample.event")]
    [KnownType(typeof(SampleEvent2))]
    public class SampleEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SampleEnum Order { get; set; }
    }

    [Contract("0.5", "sample.event2")]
    public class SampleEvent2 : SampleEvent
    {
        public bool IsGood { get; set; }
    }
}
using System;
using Astral.Runes;

namespace SampleServices
{
    [ContractName("sample.command")]
    public class SampleCommand
    {
        public int OrderId { get; set; }
        public Operation Operation { get; set; }
        public Good Good { get; set; }
    }
}
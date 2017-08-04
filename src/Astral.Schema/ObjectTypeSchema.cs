using System;

namespace Astral.Schema
{
    public class ObjectTypeSchema : ContractTypeSchema
    {
        public string Name { get; set; }

        public string TypeReference { get; set; } 

        public Version Version { get; set; }
    }

    public class ArrayTypeSchema : ContractTypeSchema
    {
        public ContractTypeSchema Element { get; set; } 
    }
}
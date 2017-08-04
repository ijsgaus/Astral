namespace Astral.Schema
{
    public class HierarchyTypeSchema : ContractTypeSchema
    {
        public string RootTypeReference { get; set; }
        public ObjectTypeSchema[] Members { get; set; } 
    }
}
namespace Astral.Schema
{
    public class CallableEndpointSchema : EndpointSchema
    {
        public ContractTypeSchema Request { get; set; }
        public ContractTypeSchema Response { get; set; }
    }
}
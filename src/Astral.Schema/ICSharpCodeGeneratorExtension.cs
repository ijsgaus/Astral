namespace Astral.Schema
{
    public interface ICSharpCodeGeneratorExtension
    {
        void WriteNamespaces(IndentWriter writer, ServiceSchema schema);
        void WriteServiceAttributes(IndentWriter writer, ServiceSchema schema);
        void WriteEndpointAttributes(IndentWriter writer, EndpointSchema endpointValue);
    }
}
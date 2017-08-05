namespace Astral.Schema.Generation
{
    public interface ICSharpCodeGeneratorExtension
    {
        void WriteNamespaces(IndentWriter writer, ServiceSchema schema);
        void WriteServiceAttributes(IndentWriter writer, ServiceSchema schema);
    }
}
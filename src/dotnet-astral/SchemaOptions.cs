using CommandLine;
using CommandLine.Text;

namespace dotnet_astral
{
    [Verb("schema", HelpText = "Generate astral schema from .net assembly")]
    public class SchemaOptions
    {
        [Option("path", HelpText = "Assembly or project path to process", Default = ".")]
        public string AssemblyPath { get; set; }
        
        [Option('o', "output", HelpText = "Output path to generate schemas", Default = "./astral")]
        public string OutputPath { get; set; }
    }
}
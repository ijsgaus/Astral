using System;
using CommandLine;

namespace dotnet_astral
{
    class Program
    {
        static int Schema(SchemaOptions options)
        {
            Console.WriteLine(options.AssemblyPath);
            Console.WriteLine(options.OutputPath);
            return 0;
        }
        
        static int CSharp(CSharpOptions options)
        {
            //Console.WriteLine(options.AssemblyPath);
            //Console.WriteLine(options.OutputPath);
            return 0;
        }
        
        static int Main(string[] args)
        {
            var result = CommandLine.Parser.Default.ParseArguments<SchemaOptions, CSharpOptions>(args)
                .MapResult<SchemaOptions, CSharpOptions, int>(Schema, CSharp, err => 1);
            Console.ReadKey();
            return result;
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.IO;
using Astral.Schema;
using Astral.Schema.Rabbit;
using SampleServices;

namespace SchemaWork
{
    class Program
    {
        static void Main(string[] args)
        {
            var schema = new ServiceSchema
            {
                Title = "Test",
                Name = "test",
                Version = Version.Parse("1.0")
            };
            schema.Endpoints.Add("event1", new EventEndpointSchema
            {
                Title = "Event1",
                Event = new ObjectTypeSchema
                {
                    
                }
            });

            schema.Endpoints.Add("cmd1", new CommandEndpointSchema
            {
                Title = "Command1"
            });

            schema.Endpoints.Add("call1", new CallableEndpointSchema()
            {
                Title = "Call1"
            });

            var json = schema.ToString();
            var obj = ServiceSchema.FromString(json);

            var generator = new SchemaGenerator<ISampleService>();
            schema = generator.Generate(new SchemaGenerationOptions(new RabbitSchemaGenerator()));
            json = schema.ToString();
            schema.ToFile("sample.json");
            schema = ServiceSchema.FromString(json);
            var csgenerator = new CSharpCodeGenerator(schema, new CSharpCodeGenerationOptions("Test")
            {
                ExcludedTypes = new[] { "SampleCommand" },
                Endpoints = new[] { "AwesomeEvent" }.Include(), 
                GeneratedProperties = new Dictionary<string, SkipInclude>
                {
                    { "SampleEvent", new [] {"Order"}.Skip() }
                }


            }, new RabbitCSharpGenerator());
            var csharp = csgenerator.GenerateContracts();
            //File.WriteAllText("sample.cs", csharp);
            var cintf = csgenerator.GenerateInterface();
            File.WriteAllText("sample.cs", csharp + "\n" + cintf);
            Console.ReadKey();
        }
    }
}
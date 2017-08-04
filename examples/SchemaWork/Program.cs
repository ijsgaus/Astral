using System;
using Astral.Schema;
using Astral.Schema.Exceptions;
using Astral.Schema.Generation;
using Astral.Schema.Rabbit;
using Newtonsoft.Json.Linq;
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
                    Name = "contract1"
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

            var generator = new SchemaGenerator<ISampleService>(new RabbitSchemaGenerator());
            schema = generator.Generate();
            json = schema.ToString();
            schema.ToFile("sample.json");

            var csharp = new CSharpCodeGenerator(schema).GenerateContracts("Test");

            Console.ReadKey();
        }
    }
}
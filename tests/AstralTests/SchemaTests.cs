using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Astral.Runes;
using Astral.Schema;
using Astral.Schema.Exceptions;
using Astral.Schema.Generation;
using Newtonsoft.Json.Linq;
using SampleServices;
using Xunit;

namespace AstralTests
{
    public class SchemaTests
    {
        [Fact]
        public void Serialization()
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
                Event = new ContractTypeSchema
                {
                    Name = "contract1",
                    Schema = new JObject(new JProperty("test", new JValue("hellow")))
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
            var restored = ServiceSchema.FromString(json);
            Assert.Equal(restored.Title, schema.Title);
            Assert.Equal(restored.Name, schema.Name);
            Assert.Equal(restored.Version, schema.Version);
            Assert.Equal(restored.Endpoints.Count, schema.Endpoints.Count);
            Assert.Equal(restored.Endpoints["event1"].GetType(), typeof(EventEndpointSchema));
            Assert.Equal(restored.Endpoints["cmd1"].GetType(), typeof(CommandEndpointSchema));
            Assert.Equal(restored.Endpoints["call1"].GetType(), typeof(CallableEndpointSchema));
        }

        [Fact]
        public void ShouldTakeOnlyInterfaces()
        {
            Assert.Throws<ArgumentException>(() => new SchemaGenerator<int>());
        }

        [Fact]
        public void ShouldDetermineVersion()
        {
            Assert.Throws<ServiceInterfaceException>(() => new SchemaGenerator<IDisposable>().Generate());
        }

        [Fact]
        public void ShouldTakeDefaultVersionWhenNotOnlyAttribute()
        {
            var generator = new SchemaGenerator<IDisposable>(useOnlyAttributes: false);
            var schema = generator.Generate();
            Assert.Equal(Version.Parse("1.0"), schema.Version);
        }

        [Fact]
        public void ShouldPreferAttributeSettedVersion()
        {
            var generator = new SchemaGenerator<ISampleService>(useOnlyAttributes: false);
            var schema = generator.Generate();
            Assert.Equal(Version.Parse("0.5"), schema.Version);
        }

        [Fact]
        public void ShouldSetTitle()
        {
            var generator = new SchemaGenerator<ISampleService>(useOnlyAttributes: false);
            var schema = generator.Generate();
            Assert.Equal(nameof(ISampleService), schema.Title);
        }

        [Fact]
        public void ShouldSetNameWhenNotAttribute()
        {
            var generator = new SchemaGenerator<INoAttributeService>(useOnlyAttributes: false);
            var schema = generator.Generate();
            Assert.Equal(nameof(INoAttributeService), schema.Name);
        }

        [Fact]
        public void ShouldSetNameWhenAttribute()
        {
            var generator = new SchemaGenerator<ISampleService>(useOnlyAttributes: false);
            var schema = generator.Generate();
            Assert.Equal(typeof(ISampleService).GetTypeInfo().GetCustomAttribute<ServiceNameAttribute>().Name, schema.Name);
        }

        [Fact]
        public void ShouldThrowWhenNoNameAttribute()
        {
            var generator = new SchemaGenerator<INoAttributeService>();
            Assert.Throws<ServiceInterfaceException>(() => generator.Generate());
        }
    }
}
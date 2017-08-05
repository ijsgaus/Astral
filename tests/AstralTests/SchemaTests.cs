using System;
using System.Reflection;
using Astral;
using Astral.Schema;
using SampleServices;
using Xunit;
using Extensions = Astral.Extensions;

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
                Event = new ObjectTypeSchema()
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
        public void ShouldSetTitle()
        {
            var generator = new SchemaGenerator<ISampleService>();
            var schema = generator.Generate();
            Assert.Equal(nameof(ISampleService), schema.Title);
        }

        [Fact]
        public void ShouldSetNameWhenNotAttribute()
        {
            var generator = new SchemaGenerator<INoAttributeService>();
            var schema = generator.Generate(new SchemaGenerationOptions
            {
                MemberNameToSchemaName = Extensions.ToDottedName
                    
            });
            Assert.Equal(nameof(INoAttributeService).ToDottedName(true), schema.Name);
        }

        [Fact]
        public void ShouldSetNameWhenAttribute()
        {
            var generator = new SchemaGenerator<ISampleService>();
            var schema = generator.Generate();
            Assert.Equal(typeof(ISampleService).GetTypeInfo().GetCustomAttribute<ServiceAttribute>().Name, schema.Name);
        }

        [Fact]
        public void ShouldThrowWhenNoNameAttribute()
        {
            var generator = new SchemaGenerator<INoAttributeService>();
            Assert.Throws<ServiceInterfaceException>(() => generator.Generate());
        }

        [Fact]
        public void ShouldGenerateDottedNameForInterfaces()
        {
            Assert.Equal("no.attribute.service", typeof(INoAttributeService).Name.ToDottedName(true));
        }

        [Fact]
        public void ShouldGenerateDottedNameForOthers()
        {
            Assert.Equal("inform.translate", "InformTranslate".ToDottedName(false));
        }


    }
}
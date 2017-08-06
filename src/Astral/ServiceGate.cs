using System;
using System.Linq.Expressions;
using Astral.Configuraiton;
using Astral.Gates;
using Astral.Schema;

namespace Astral
{
    internal class ServiceGate<T> : IServiceGate<T>
    {
        private readonly GateConfig _config;
        private readonly ServiceSchema _schema;

        internal ServiceGate(GateConfig config, ServiceSchema schema)
        {
            _config = config;
            _schema = schema;
        }

        public IEvent<TEvent> Endpoint<TEvent>(Expression<Func<T, IEvent<TEvent>>> selector)
        {
            throw new NotImplementedException();
        }
    }
}
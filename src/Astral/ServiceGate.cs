using System;
using System.Linq.Expressions;
using Astral.Configuraiton;
using Astral.Gates;
using Astral.Schema;

namespace Astral
{
    internal class ServiceGate<T> : IServiceGate<T>
    {
        private readonly ServiceConfig<T> _config;


        internal ServiceGate(ServiceConfig<T> config)
        {
            _config = config;
        }

        public IEvent<TEvent> Endpoint<TEvent>(Expression<Func<T, IEvent<TEvent>>> selector)
        {
            throw new NotImplementedException();
        }
    }
}
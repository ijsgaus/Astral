using Astral.Configuraiton;
using Astral.Schema;
using Astral.Transports;
using Microsoft.Extensions.Logging;

namespace Astral.Endpoints
{
    public abstract class EndpointBase : ILoggable
    {
        private readonly EndpointConfig _config;
        

        protected EndpointBase(ILoggerFactory loggerFactory, EndpointConfig config)
        {
            _config = config;
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        protected ILoggerFactory LoggerFactory { get;  }
        public ILogger Logger { get; }

        
    }
}
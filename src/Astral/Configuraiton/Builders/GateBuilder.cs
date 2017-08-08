using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading;
using Astral.Schema;
using Astral.Transports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Astral.Configuraiton
{
    public class GateBuilder
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly SchemaGenerationOptions _options;


        private TransportConfig _transportConfig;

        public GateBuilder(ILoggerFactory loggerFactory, SchemaGenerationOptions options = null)
        {
            _loggerFactory = loggerFactory;
            _options = options ?? new SchemaGenerationOptions();
            _transportConfig = new TransportConfig();
        }

        public void RegisterTransport<T>(string porterCode, T porter, TransportType? type = null, bool asDefault = true)
            where T : ITransport
            => _transportConfig.RegisterTransport(porterCode, porter, type, asDefault);

        public Gate Build()
        {
            var gate =  new Gate(_loggerFactory, new GateConfig(_options, _transportConfig));
            _transportConfig = new TransportConfig();
            return gate;
        }
    }
}
using Astral.Transports;

namespace Astral.Configuraiton
{
    public abstract class EndpointConfig
    {
        internal T GetTransport<T>()
            where T : ITransport
        {
            throw new System.NotImplementedException();
        }
    }
}
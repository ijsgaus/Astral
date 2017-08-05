using Microsoft.Extensions.Logging;

namespace Astral
{
    public interface ILoggable
    {
        ILogger Logger { get; }
    }
}
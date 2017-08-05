using System;

namespace Astral
{
    public interface IConnector
    {
        IDisposable Connect();
    }
}
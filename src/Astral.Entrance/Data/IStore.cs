using System.Data;

namespace Astral.Data
{
    /// <summary>
    ///     Provide interface for starting unit of work
    /// </summary>
    public interface IStore<out T> where T : IUnitOfWork
    {
        /// <summary>
        ///     Start unit of work
        /// </summary>
        /// <param name="isolationLevel">level of isolation</param>
        /// <returns>unit of work</returns>
        T BeginWork(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
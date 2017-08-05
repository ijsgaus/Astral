using System;

namespace Astral.Data
{
    /// <inheritdoc />
    /// <summary>
    ///     Unit of work
    /// </summary>
    public interface IUnitOfWork : IDisposable //, IRegisterAfterCommit
    {
        /// <summary>
        ///     Commit changes. When not called, Dispose must rollback
        /// </summary>
        void Commit();
    }
}
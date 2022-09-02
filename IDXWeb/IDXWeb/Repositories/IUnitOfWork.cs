using System;

namespace IDXWeb.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        INewsRepository NewsRepository { get; }
        void Commit();
    }
}

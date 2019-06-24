using System;

namespace CarAdvertiser.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
    }
}

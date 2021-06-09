using System;
using DowntimeAlerter.Data.Repository;
using DowntimeAlerter.Domain.Entities;

namespace DowntimeAlerter.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Application> Applications { get; }
        int Complete();
        int Complete(bool usingTransaction);
        IUnitOfWork New();
    }
}
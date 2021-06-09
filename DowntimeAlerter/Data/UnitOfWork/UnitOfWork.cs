using System;
using DowntimeAlerter.Data.DbContext;
using DowntimeAlerter.Data.Repository;
using DowntimeAlerter.Domain.Entities;
using DowntimeAlerter.Infrastructure.Helper;
using Microsoft.EntityFrameworkCore.Storage;

namespace DowntimeAlerter.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private Microsoft.EntityFrameworkCore.DbContext _context = null;
        private readonly Type ContextType;


        public UnitOfWork()
        {
            ContextType = typeof(ApplicationDbContext);
        }

        private IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public IGenericRepository<Application> Applications => new GenericRepository<Application>(_context);

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }


        public int Complete()
        {
            return _context.SaveChanges();
        }

        public int Complete(bool usingTransaction)
        {
            if (!usingTransaction)
                return Complete();

            int status;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    status = _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return status;
        }

        public IUnitOfWork New()
        {
            _context = null;
            TryCreateContext();
            return this;
        }

        void TryCreateContext()
        {
            if (ContextType == default(Type))
                throw new CustomException("Context type Unknown");

            if (_context == null)
                _context = Activator.CreateInstance(ContextType) as ApplicationDbContext;
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DowntimeAlerter.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly Microsoft.EntityFrameworkCore.DbContext _context;
        private readonly DbSet<T> _table;

        public GenericRepository(Microsoft.EntityFrameworkCore.DbContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _table.ToListAsync();
        }

        public async Task<T> GetById(object id)
        {
            return await _table.FindAsync(id);
        }

        public T Add(T obj)
        {
            _table.Add(obj);
            return obj;
        }

        public T Update(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
            _table.Update(obj);
            return obj;
        }

        public T Delete(object id)
        {
            T existing = _table.Find(id);
            if (existing != null)
                _table.Remove(existing);
            return existing;
        }

        public T SoftDelete(object id)
        {
            T existing = _table.Find(id);
            if (existing == null) return null;
            var property = existing.GetType().GetProperty("IsDeleted");
            var propertyValue = (bool?) property?.GetValue(existing);

            if (propertyValue.HasValue) return existing;
            property?.SetValue(existing, true);
            _context.Entry(existing).State = EntityState.Modified;
            _table.Update(existing);

            return existing;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
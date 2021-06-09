using System.Collections.Generic;
using System.Threading.Tasks;

namespace DowntimeAlerter.Data.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(object id);
        T Add(T obj);
        T Update(T obj);
        T Delete(object id);
        T SoftDelete(object id);
        Task SaveAsync();
        void Dispose();
    }
}
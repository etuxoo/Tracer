using System.Collections.Generic;
using System.Threading.Tasks;

namespace TraceService.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T: class
    {
        Task<T> GetByTrnAsync(string trn);
        Task<T> GetByMtiAsync(string mti);
        Task<long> AddAsync(T entity);
    }
}

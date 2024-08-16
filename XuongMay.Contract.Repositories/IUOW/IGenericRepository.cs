using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XuongMay.Core;


namespace XuongMay.Contract.Repositories.IUOW
{
    public interface IGenericRepository<T> where T : class
    {
        // Query
        IQueryable<T> Entities { get; }

        // Non-async methods
        IEnumerable<T> GetAll();
        T? GetById(ObjectId id);
        void Insert(T obj);
        void InsertRange(IList<T> obj);
        void Update(T obj);
        void Delete(ObjectId id);
        void Save();

        // Async methods
        Task<IList<T>> GetAllAsync();
        Task<BasePaginatedList<T>> GetPaginatedAsync(IQueryable<T> query, int pageIndex, int pageSize);
        Task<T?> GetByIdAsync(ObjectId id);
        Task InsertAsync(T obj);
        Task InsertRangeAsync(IList<T> obj);
        Task UpdateAsync(T obj);
        Task DeleteAsync(ObjectId id);
        Task SaveAsync();
    }
}

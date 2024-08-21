using MongoDB.Bson;
using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Contract.Repositories.IUOW
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(Guid id);
        Task CreateAsync(Product product);
        Task<bool> UpdateAsync(Guid id, Product product);
        Task<bool> DeleteAsync(Guid id);
    }
}

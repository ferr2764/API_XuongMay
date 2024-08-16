using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Contract.Repositories.IUOW
{
    public interface IProductRepository
    {
        // Get all products
        Task<IEnumerable<Product>> GetAllAsync();

        // Get product by Id
        Task<Product> GetByIdAsync(ObjectId id);

        // Create a new product
        Task CreateAsync(Product product);

        // Update an existing product
        Task<bool> UpdateAsync(ObjectId id, Product product);

        // Delete a product by Id
        Task<bool> DeleteAsync(ObjectId id);
    }
}

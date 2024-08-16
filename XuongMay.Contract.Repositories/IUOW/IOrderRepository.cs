using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace XuongMay.Contract.Repositories.IUOW
{
    public interface IOrderRepository
    {
        // Get all orders
        Task<IEnumerable<Order>> GetAllAsync();

        // Get order by Id
        Task<Order> GetByIdAsync(ObjectId id);

        // Create a new order
        Task CreateAsync(Order order);

        // Update an existing order
        Task<bool> UpdateAsync(ObjectId id, Order order);

        // Delete an order by Id
        Task<bool> DeleteAsync(ObjectId id);
    }
}

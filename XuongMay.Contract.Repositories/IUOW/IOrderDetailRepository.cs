using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using XuongMay.Contract.Repositories.Entity;


namespace XuongMay.Contract.Repositories.IUOW
{
    public interface IOrderDetailRepository
    {
        // Get all order details
        Task<IEnumerable<OrderDetail>> GetAllAsync();

        // Get order detail by Id
        Task<OrderDetail> GetByIdAsync(ObjectId id);

        // Create a new order detail
        Task CreateAsync(OrderDetail orderDetail);

        // Update an existing order detail
        Task<bool> UpdateAsync(ObjectId id, OrderDetail orderDetail);

        // Delete an order detail by Id
        Task<bool> DeleteAsync(ObjectId id);

    }
}

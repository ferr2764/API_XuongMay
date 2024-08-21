using MongoDB.Bson;
using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Contract.Repositories.IUOW
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(Guid id);
        Task CreateAsync(Order order);
        Task<bool> UpdateAsync(Guid id, Order order);
        Task<bool> DeleteAsync(Guid id);
    }
}

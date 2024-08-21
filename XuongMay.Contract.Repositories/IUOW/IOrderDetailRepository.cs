using MongoDB.Bson;
using XuongMay.Contract.Repositories.Entity;


namespace XuongMay.Contract.Repositories.IUOW
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetAllAsync();
        Task<OrderDetail> GetByIdAsync(Guid id);
        Task CreateAsync(OrderDetail orderDetail);
        Task<bool> UpdateAsync(Guid id, OrderDetail orderDetail);
        Task<bool> DeleteAsync(Guid id);
    }
}

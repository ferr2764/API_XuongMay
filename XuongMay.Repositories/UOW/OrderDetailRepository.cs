using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using XuongMay.Contract.Repositories;
using XuongMay.Contract.Repositories.IUOW;
using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Repositories.UOW
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly IMongoCollection<OrderDetail> _orderDetails;

        public OrderDetailRepository(IMongoDatabase database)
        {
            _orderDetails = database.GetCollection<OrderDetail>("OrderDetails");
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await _orderDetails.Find(_ => true).ToListAsync();
        }

        public async Task<OrderDetail> GetByIdAsync(ObjectId id)
        {
            return await _orderDetails.Find(orderDetail => orderDetail.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(OrderDetail orderDetail)
        {
            await _orderDetails.InsertOneAsync(orderDetail);
        }

        public async Task<bool> UpdateAsync(ObjectId id, OrderDetail orderDetail)
        {
            var result = await _orderDetails.ReplaceOneAsync(od => od.Id == id, orderDetail);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(ObjectId id)
        {
            var result = await _orderDetails.DeleteOneAsync(od => od.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
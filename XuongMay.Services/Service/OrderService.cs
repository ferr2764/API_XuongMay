using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Repositories.Interface;
using XuongMay.Contract.Services.Interface;

namespace XuongMay.Services.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var repository = _unitOfWork.GetRepository<Order>();
            return await repository.GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<Order>();
            return await repository.GetByIdAsync(objectId);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            var repository = _unitOfWork.GetRepository<Order>();
            await repository.InsertAsync(order);
            await _unitOfWork.SaveAsync();
            return order;
        }

        public async Task<Order> UpdateOrderAsync(string id, Order order)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<Order>();
            var existingOrder = await repository.GetByIdAsync(objectId);
            if (existingOrder == null)
                return null;

            // Update các thuộc tính cần thiết
            existingOrder.Status = order.Status;
            existingOrder.Deadline = order.Deadline;
            existingOrder.FinishDate = order.FinishDate;
            existingOrder.AssignedAccountId = order.AssignedAccountId;

            repository.Update(existingOrder);
            await _unitOfWork.SaveAsync();

            return existingOrder;
        }

        public async Task<bool> DeleteOrderAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return false;

            var repository = _unitOfWork.GetRepository<Order>();
            var order = await repository.GetByIdAsync(objectId);
            if (order == null)
                return false;

            await repository.DeleteAsync(objectId);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}

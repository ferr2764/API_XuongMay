using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Repositories.Interface;
using XuongMay.Contract.Services.Interface;
using XuongMay.ModelViews.OrderModelViews;

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

        public async Task<Order> CreateOrderAsync(CreateOrderModelView orderViewModel)
        {
            Order order = new Order();
            order.AccountId = ObjectId.Parse(orderViewModel.AccountId);
            order.Deadline = orderViewModel.Deadline;
            order.Status = "Created";
            order.CreatedDate = DateTime.Now;
            var repository = _unitOfWork.GetRepository<Order>();
            await repository.InsertAsync(order);
            //await _unitOfWork.SaveAsync();
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
            var existingOrder = await repository.GetByIdAsync(objectId);
            if (existingOrder == null)
                return false;

            // Update trạng thái thành Unavailable
            existingOrder.Status = "Unavailable";
            

            repository.Update(existingOrder);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}

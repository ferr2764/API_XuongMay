using MongoDB.Bson;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Order>> GetPaginatedOrdersAsync(int pageNumber, int pageSize)
        {
            var repository = _unitOfWork.GetRepository<Order>();
            var orders = await repository.GetAllAsync();

            var pagedOrders = orders
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();

            return pagedOrders;
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
            Order order = new Order
            {
                AccountId = ObjectId.Parse(orderViewModel.AccountId),
                Deadline = orderViewModel.Deadline,
                Status = "Created",
                CreatedDate = DateTime.Now
            };

            var repository = _unitOfWork.GetRepository<Order>();
            await repository.InsertAsync(order);
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

        public async Task<Order> MoveToNextStatusAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<Order>();
            var order = await repository.GetByIdAsync(objectId);
            if (order == null)
                return null;

            // Chuyển trạng thái theo thứ tự
            switch (order.Status)
            {
                case "Created":
                    order.Status = "Assigned";
                    break;
                case "Assigned":
                    order.Status = "Completed";
                    order.FinishDate = DateTime.Now;
                    break;
                case "Completed":
                    return null; // Không thể chuyển trạng thái từ Completed
                default:
                    return null;
            }

            repository.Update(order);
            //await _unitOfWork.SaveAsync();

            return order;
        }

        public async Task<bool> CancelOrderAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return false;

            var repository = _unitOfWork.GetRepository<Order>();
            var order = await repository.GetByIdAsync(objectId);
            if (order == null || order.Status == "Completed")
                return false;

            // Hủy đơn hàng
            order.Status = "Cancelled";

            repository.Update(order);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<Order> AssignOrderAsync(AssignOrderModelView assignOrderModelView, string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<Order>();
            var order = await repository.GetByIdAsync(objectId);
            if (order == null)
                return null;

            // Gán đơn hàng cho nhân viên
            order.AssignedAccountId = ObjectId.Parse(assignOrderModelView.AccountId);
            order.Status = "Assigned";

            repository.Update(order);
            //await _unitOfWork.SaveAsync();

            return order;
        }

        public async Task UpdateOrderAsync(Order order)
        {
            var repository = _unitOfWork.GetRepository<Order>();
            repository.Update(order);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateOrderDetailAsync(OrderDetail detail)
        {
            var repository = _unitOfWork.GetRepository<OrderDetail>();
            repository.Update(detail);
            await _unitOfWork.SaveAsync();
        }
    }
}

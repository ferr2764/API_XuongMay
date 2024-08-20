using MongoDB.Bson;
using MongoDB.Driver;
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
            {
                return null;
            }

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
                CreatedDate = DateTime.UtcNow
            };

            var repository = _unitOfWork.GetRepository<Order>();
            await repository.InsertAsync(order);
            return order;
        }

        public async Task<Order> UpdateOrderAsync(string id, UpdateOrderModelView order)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return null;
            }

            var repository = _unitOfWork.GetRepository<Order>();
            var existingOrder = await repository.GetByIdAsync(objectId);
            if (existingOrder == null)
            {
                return null;
            }

            existingOrder.Status = order.Status;
            existingOrder.Deadline = order.Deadline;
            existingOrder.FinishDate = order.FinishDate;
            existingOrder.AssignedAccountId = string.IsNullOrEmpty(order.AssignedAccountId)
                                              ? existingOrder.AssignedAccountId
                                              : ObjectId.Parse(order.AssignedAccountId);


            await repository.UpdateAsync(existingOrder);

            return existingOrder;
        }

        public async Task<bool> DeleteOrderAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return false;
            }

            var repository = _unitOfWork.GetRepository<Order>();
            var existingOrder = await repository.GetByIdAsync(objectId);
            if (existingOrder == null)
            {
                return false;
            }

            // Update trạng thái thành Unavailable
            existingOrder.Status = "Unavailable";

            await repository.UpdateAsync(existingOrder);

            return true;
        }

        public async Task<Order> MoveToNextStatusAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return null;
            }

            var repository = _unitOfWork.GetRepository<Order>();
            var order = await repository.GetByIdAsync(objectId);
            if (order == null)
            {
                return null;
            }

            // Chuyển trạng thái theo thứ tự
            switch (order.Status)
            {
                case "Created":
                    order.Status = "Assigned";
                    break;
                case "Assigned":
                    order.Status = "Completed";
                    order.FinishDate = DateTime.UtcNow;
                    break;
                case "Completed":
                    return null; // Không thể chuyển trạng thái từ Completed
                default:
                    return null;
            }
            await repository.UpdateAsync(order);


            return order;
        }

        public async Task<bool> CancelOrderAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return false;
            }

            var orderRepository = _unitOfWork.GetRepository<Order>();
            var order = await orderRepository.GetByIdAsync(objectId);
            if (order == null || order.Status == "Completed" || order.Status == "Unavailable" || order.Status == "Cancelled")
            {
                return false;
            }

            order.Status = "Cancelled";
            orderRepository.Update(order);

            var orderDetailRepository = _unitOfWork.GetRepository<OrderDetail>();
            var filter = Builders<OrderDetail>.Filter.Eq(od => od.OrderId, objectId);
            var orderDetails = await orderDetailRepository.GetAllByFilterAsync(filter);

            foreach (var orderDetail in orderDetails)
            {
                if (orderDetail.Status != "Completed")
                {
                    orderDetail.Status = "Canceled";
                    orderDetailRepository.Update(orderDetail);
                }
            }


            return true;
        }


        public async Task<Order> AssignOrderAsync(AssignOrderModelView assignOrderModelView, string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return null;
            }

            var repository = _unitOfWork.GetRepository<Order>();
            var order = await repository.GetByIdAsync(objectId);
            if (order == null)
            {
                return null;
            }

            var accountRepository = _unitOfWork.GetRepository<Account>();
            var account = await accountRepository.GetByIdAsync(ObjectId.Parse(assignOrderModelView.AccountId));

            if (account == null || account.Status == "Unavailable")
            {
                throw new Exception("Cannot assign order to an unavailable account.");
            }

            // Check if the account has the role "Manager"

            if (account.Role == "Manager")
            {
                throw new Exception("Cannot assign order to an account with the role 'Manager'.");
            }

            order.AssignedAccountId = ObjectId.Parse(assignOrderModelView.AccountId);
            order.Status = "Assigned";
            await repository.UpdateAsync(order);
            return order;
        }
    }
}

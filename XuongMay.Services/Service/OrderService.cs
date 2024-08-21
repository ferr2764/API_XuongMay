using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            var repository = _unitOfWork.GetRepository<Order>();
            return await repository.GetByIdAsync(id);
        }

        public async Task<Order> CreateOrderAsync(CreateOrderModelView orderViewModel)
        {
            var order = new Order
            {
                AccountId = orderViewModel.AccountId,
                Deadline = orderViewModel.Deadline,
                Status = "Created",
                CreatedDate = DateTime.UtcNow
            };

            var repository = _unitOfWork.GetRepository<Order>();
            await repository.InsertAsync(order);
            return order;
        }

        public async Task<Order> UpdateOrderAsync(Guid id, UpdateOrderModelView order)
        {
            var repository = _unitOfWork.GetRepository<Order>();
            var existingOrder = await repository.GetByIdAsync(id);
            if (existingOrder == null)
            {
                return null;
            }

            existingOrder.Status = order.Status;
            existingOrder.Deadline = order.Deadline;
            existingOrder.FinishDate = order.FinishDate;
            existingOrder.AssignedAccountId = string.IsNullOrEmpty(order.AssignedAccountId.ToString())
                                              ? existingOrder.AssignedAccountId
                                              : order.AssignedAccountId;

            await repository.UpdateAsync(existingOrder);

            return existingOrder;
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            var repository = _unitOfWork.GetRepository<Order>();
            var existingOrder = await repository.GetByIdAsync(id);
            if (existingOrder == null)
            {
                return false;
            }

            // Update status to Unavailable
            existingOrder.Status = "Unavailable";

            await repository.UpdateAsync(existingOrder);

            return true;
        }

        public async Task<Order> MoveToNextStatusAsync(Guid id)
        {
            var repository = _unitOfWork.GetRepository<Order>();
            var order = await repository.GetByIdAsync(id);
            if (order == null)
            {
                return null;
            }

            // Change status in order
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
                    return null; // Cannot move from Completed
                default:
                    return null;
            }
            await repository.UpdateAsync(order);

            return order;
        }

        public async Task<bool> CancelOrderAsync(Guid id)
        {
            var orderRepository = _unitOfWork.GetRepository<Order>();
            var order = await orderRepository.GetByIdAsync(id);
            if (order == null || order.Status == "Completed" || order.Status == "Unavailable" || order.Status == "Cancelled")
            {
                return false;
            }

            order.Status = "Cancelled";
            await orderRepository.UpdateAsync(order);

            var orderDetailRepository = _unitOfWork.GetRepository<OrderDetail>();
            var orderDetails = await orderDetailRepository.GetAllByFilterAsync(od => od.OrderId == id);

            foreach (var orderDetail in orderDetails)
            {
                if (orderDetail.Status != "Completed")
                {
                    orderDetail.Status = "Cancelled";
                    await orderDetailRepository.UpdateAsync(orderDetail);
                }
            }

            return true;
        }

        public async Task<Order> AssignOrderAsync(AssignOrderModelView assignOrderModelView, Guid id)
        {
            var repository = _unitOfWork.GetRepository<Order>();
            var order = await repository.GetByIdAsync(id);
            if (order == null)
            {
                return null;
            }

            var accountRepository = _unitOfWork.GetRepository<Account>();
            var account = await accountRepository.GetByIdAsync(assignOrderModelView.AccountId);

            if (account == null || account.Status == "Unavailable")
            {
                throw new Exception("Cannot assign order to an unavailable account.");
            }

            // Check if the account has the role "Manager"
            if (account.Role == "Manager")
            {
                throw new Exception("Cannot assign order to an account with the role 'Manager'.");
            }

            order.AssignedAccountId = assignOrderModelView.AccountId;
            order.Status = "Assigned";
            await repository.UpdateAsync(order);
            return order;
        }
    }
}

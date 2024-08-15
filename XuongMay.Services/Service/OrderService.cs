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

        public async Task<Order> CreateOrderAsync(Order order)
        {
            foreach (var detail in order.OrderDetails)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId);
                if (product == null)
                {
                    throw new Exception("Product not found.");
                }

                var existingCategory = await _unitOfWork.Categories.GetByIdAsync(product.CategoryId);

                if (existingCategory == null)
                {
                    await _unitOfWork.Categories.InsertAsync(product.Category);
                }
            }

            await _unitOfWork.Orders.InsertAsync(order);
            await _unitOfWork.CompleteAsync();

            return order;
        }

        public async Task AssignOrderToStaffAsync(int orderId, int staffId)
        {
            var staff = await _unitOfWork.Accounts.GetByIdAsync(staffId);
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);

            if (staff == null || order == null)
            {
                throw new Exception("Staff or Order not found.");
            }

            if (staff.AssignedOrders.Any(o => o.Status != 3)) // Assuming 3 is the 'Completed' status
            {
                throw new Exception("Staff has not completed the previous order.");
            }

            staff.AssignedOrders.Add(order);
            order.Status = 1; // Set status to 'Assigned'

            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, int newStatus)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            order.Status = newStatus;
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _unitOfWork.Orders.GetByIdAsync(id);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Contract.Services.Interface
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task AssignOrderToStaffAsync(int orderId, int staffId);
        Task UpdateOrderStatusAsync(int orderId, int newStatus);
        Task<Order?> GetOrderByIdAsync(int orderId);
    }
}

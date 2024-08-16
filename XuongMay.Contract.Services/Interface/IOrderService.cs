using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.ModelViews.OrderModelViews;

namespace XuongMay.Contract.Services.Interface
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(string id);
        Task<Order> CreateOrderAsync(CreateOrderModelView orderViewModel);
        Task<Order> UpdateOrderAsync(string id, Order order);
        Task<bool> DeleteOrderAsync(string id);
    }
}

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
        Task<IEnumerable<Order>> GetPaginatedOrdersAsync(int pageNumber, int pageSize);
        Task<Order> GetOrderByIdAsync(string id);
        Task<Order> CreateOrderAsync(CreateOrderModelView orderViewModel);
        Task<Order> UpdateOrderAsync(string id, UpdateOrderModelView order);
        Task<bool> DeleteOrderAsync(string id);
        Task<Order> MoveToNextStatusAsync(string id);
        Task<bool> CancelOrderAsync(string id);
        Task<Order> AssignOrderAsync(AssignOrderModelView assignOrderModelView, string id);
    }
}

using XuongMay.Contract.Repositories.Entity;
using XuongMay.ModelViews.OrderModelViews;

namespace XuongMay.Contract.Services.Interface
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetPaginatedOrdersAsync(int pageNumber, int pageSize);
        Task<Order> GetOrderByIdAsync(Guid id);
        Task<Order> CreateOrderAsync(CreateOrderModelView orderViewModel);
        Task<Order> UpdateOrderAsync(Guid id, UpdateOrderModelView order);
        Task<bool> DeleteOrderAsync(Guid id);
        Task<Order> MoveToNextStatusAsync(Guid id);
        Task<bool> CancelOrderAsync(Guid id);
        Task<Order> AssignOrderAsync(AssignOrderModelView assignOrderModelView, Guid id);
    }
}

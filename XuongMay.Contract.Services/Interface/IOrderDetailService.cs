using XuongMay.Contract.Repositories.Entity;
using XuongMay.ModelViews.OrderDetailModelView;

namespace XuongMay.Contract.Services.Interface
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetPaginatedOrderDetailsAsync(int pageNumber, int pageSize);
        Task<OrderDetail> GetOrderDetailByIdAsync(Guid id);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(Guid orderId);
        Task<OrderDetail> CreateOrderDetailAsync(CreateOrderDetailModelView orderDetailModel);
        Task<OrderDetail> UpdateOrderDetailAsync(Guid id, UpdateOrderDetailModelView orderDetailModel);
        Task<bool> DeleteOrderDetailAsync(Guid id);
        Task<OrderDetail> CancelOrderDetailAsync(Guid id);
        Task<OrderDetail> MoveToNextStatusAsync(Guid id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Repositories.Interface;
using XuongMay.Contract.Services.Interface;
using XuongMay.ModelViews.OrderDetailModelView;

namespace XuongMay.Services.Service
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<OrderDetail>> GetPaginatedOrderDetailsAsync(int pageNumber, int pageSize)
        {
            var repository = _unitOfWork.GetRepository<OrderDetail>();
            var orderDetails = await repository.GetAllAsync();
            var pagedOrderDetails = orderDetails
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return pagedOrderDetails;
        }

        public async Task<OrderDetail> GetOrderDetailByIdAsync(Guid id)
        {
            var repository = _unitOfWork.GetRepository<OrderDetail>();
            return await repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(Guid orderId)
        {
            var repository = _unitOfWork.GetRepository<OrderDetail>();
            return await repository.GetAllByFilterAsync(od => od.OrderId == orderId);
        }

        public async Task<OrderDetail> CreateOrderDetailAsync(CreateOrderDetailModelView orderDetailModelView)
        {
            var orderDetail = new OrderDetail
            {
                DetailId = Guid.NewGuid(),
                OrderId = orderDetailModelView.OrderId,
                ProductId = orderDetailModelView.ProductId,
                NumberOfProds = orderDetailModelView.NumberOfProds
            };

            var repository = _unitOfWork.GetRepository<OrderDetail>();
            await repository.InsertAsync(orderDetail);
            return orderDetail;
        }

        public async Task<OrderDetail> UpdateOrderDetailAsync(Guid id, UpdateOrderDetailModelView orderDetailModelView)
        {
            var repository = _unitOfWork.GetRepository<OrderDetail>();
            var existingOrderDetail = await repository.GetByIdAsync(id);
            if (existingOrderDetail == null) return null;

            existingOrderDetail.NumberOfProds = orderDetailModelView.NumberOfProds;
            existingOrderDetail.ProductId = orderDetailModelView.ProductId;
            existingOrderDetail.Status = orderDetailModelView.Status;

            await repository.UpdateAsync(existingOrderDetail);
            return existingOrderDetail;
        }

        public async Task<bool> DeleteOrderDetailAsync(Guid id)
        {
            var repository = _unitOfWork.GetRepository<OrderDetail>();
            var existingOrderDetail = await repository.GetByIdAsync(id);
            if (existingOrderDetail == null) return false;

            await repository.DeleteAsync(id);
            return true;
        }

        public async Task<OrderDetail> CancelOrderDetailAsync(Guid id)
        {
            var repository = _unitOfWork.GetRepository<OrderDetail>();
            var existingOrderDetail = await repository.GetByIdAsync(id);
            if (existingOrderDetail == null) return null;

            // Set the status to "Cancelled" or similar logic
            existingOrderDetail.Status = "Cancelled";
            await repository.UpdateAsync(existingOrderDetail);

            return existingOrderDetail;
        }

        public async Task<OrderDetail> MoveToNextStatusAsync(Guid id)
        {
            var repository = _unitOfWork.GetRepository<OrderDetail>();
            var existingOrderDetail = await repository.GetByIdAsync(id);
            if (existingOrderDetail == null) return null;

            // Implement logic to move to the next status
            switch (existingOrderDetail.Status)
            {
                case "Pending":
                    existingOrderDetail.Status = "InProgress";
                    break;
                case "InProgress":
                    existingOrderDetail.Status = "Completed";
                    break;
                case "Completed":
                    return null; // Cannot move further
                default:
                    return null;
            }
            await repository.UpdateAsync(existingOrderDetail);

            return existingOrderDetail;
        }
    }
}

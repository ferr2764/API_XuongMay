using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            // Apply pagination using Skip and Take
            var pagedOrderDetails = orderDetails
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

            return pagedOrderDetails;
        }


        public async Task<OrderDetail> GetOrderDetailByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<OrderDetail>();
            return await repository.GetByIdAsync(objectId);
        }

        public async Task<OrderDetail> CreateOrderDetailAsync(CreateOrderDetailModelView orderDetailModel)
        {

            // Fetch the product from the repository
            var productRepository = _unitOfWork.GetRepository<Product>();
            var product = await productRepository.GetByIdAsync(ObjectId.Parse(orderDetailModel.ProductId));

            // Validate the product status
            if (product == null || product.Status != "Available")
            {
                throw new Exception("Cannot create an order detail for a product that is unavailable.");
            }
            OrderDetail orderDetail = new OrderDetail
            {
                OrderId = ObjectId.Parse(orderDetailModel.OrderId),
                ProductId = ObjectId.Parse(orderDetailModel.ProductId),
                Status = "Created",
                NumberOfProds = orderDetailModel.NumberOfProds
            };

            var repository = _unitOfWork.GetRepository<OrderDetail>();
            await repository.InsertAsync(orderDetail);
            //await _unitOfWork.SaveAsync();
            return orderDetail;
        }

        public async Task<OrderDetail> UpdateOrderDetailAsync(string id, OrderDetail orderDetail)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<OrderDetail>();
            var existingOrderDetail = await repository.GetByIdAsync(objectId);
            if (existingOrderDetail == null)
                return null;

            existingOrderDetail.Status = orderDetail.Status;
            existingOrderDetail.NumberOfProds = orderDetail.NumberOfProds;

            repository.Update(existingOrderDetail);
            await _unitOfWork.SaveAsync();

            return existingOrderDetail;
        }

        public async Task<bool> DeleteOrderDetailAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return false;

            var repository = _unitOfWork.GetRepository<OrderDetail>();
            var existingOrderDetail = await repository.GetByIdAsync(objectId);
            if (existingOrderDetail == null)
                return false;

            // Update trạng thái thành Unavailable
            existingOrderDetail.Status = "Unavailable";

            repository.Update(existingOrderDetail);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<OrderDetail> CancelOrderDetailAsync(string id)
        {
            OrderDetail orderDetail = new();
            orderDetail.Id = ObjectId.Parse(id);
            var repository = _unitOfWork.GetRepository<OrderDetail>();
            var existingOrderDetail = await repository.GetByIdAsync(orderDetail.Id);
            if (existingOrderDetail == null)
                return null;
            existingOrderDetail.Status = "Canceled";

            repository.Update(existingOrderDetail);
            //await _unitOfWork.SaveAsync();

            return existingOrderDetail;
        }
    }
}

using MongoDB.Bson;
using MongoDB.Driver;
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

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(string orderId)
        {
            if (!ObjectId.TryParse(orderId, out var objectId))
                return Enumerable.Empty<OrderDetail>();

            var repository = _unitOfWork.GetRepository<OrderDetail>();
            var allOrderDetails = await repository.GetAllAsync();

            return allOrderDetails.Where(od => od.OrderId == objectId);
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

        public async Task<OrderDetail> UpdateOrderDetailAsync(string id, UpdateOrderDetailModelView orderDetailModel)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<OrderDetail>();
            var existingOrderDetail = await repository.GetByIdAsync(objectId);
            if (existingOrderDetail == null)
                return null;

            // Fetch and validate the new product from the repository
            if (!ObjectId.TryParse(orderDetailModel.ProductId, out var newProductId))
            {
                throw new Exception("Invalid Product ID.");
            }

            var productRepository = _unitOfWork.GetRepository<Product>();
            var newProduct = await productRepository.GetByIdAsync(newProductId);

            if (newProduct == null || newProduct.Status != "Available")
            {
                throw new Exception("Cannot assign an unavailable product to the order detail.");
            }

            // Update the relevant fields
            existingOrderDetail.ProductId = newProductId;
            existingOrderDetail.Status = orderDetailModel.Status;
            existingOrderDetail.NumberOfProds = orderDetailModel.NumberOfProds;

            await repository.UpdateAsync(existingOrderDetail);

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

            await repository.UpdateAsync(existingOrderDetail);

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

            await repository.UpdateAsync(existingOrderDetail);

            return existingOrderDetail;
        }

        public async Task<OrderDetail> MoveToNextStatusAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<OrderDetail>();
            var orderDetail = await repository.GetByIdAsync(objectId);
            if (orderDetail == null)
                return null;
            if (orderDetail.Status.Equals("Created"))
            {
                orderDetail.Status = "Completed";
            }
            else return null;

            await repository.UpdateAsync(orderDetail);

            return orderDetail;
        }
    }
}

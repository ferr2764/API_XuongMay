﻿using MongoDB.Bson;
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

        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync()
        {
            var repository = _unitOfWork.GetRepository<OrderDetail>();
            return await repository.GetAllAsync();
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
            OrderDetail orderDetail = new();
            orderDetail.OrderId = ObjectId.Parse(orderDetailModel.OrderId);
            orderDetail.ProductId = ObjectId.Parse(orderDetailModel.ProductId);
            orderDetail.Status = "Created";
            orderDetail.NumberOfProds = orderDetailModel.NumberOfProds;

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

            // Update các thuộc tính cần thiết
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
            var orderDetail = await repository.GetByIdAsync(objectId);
            if (orderDetail == null)
                return false;

            await repository.DeleteAsync(objectId);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}

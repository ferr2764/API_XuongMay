﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.ModelViews.OrderDetailModelView;

namespace XuongMay.Contract.Services.Interface
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync();
        Task<OrderDetail> GetOrderDetailByIdAsync(string id);
        Task<OrderDetail> CreateOrderDetailAsync(CreateOrderDetailModelView orderDetail);
        Task<OrderDetail> UpdateOrderDetailAsync(string id, OrderDetail orderDetail);
        Task<bool> DeleteOrderDetailAsync(string id);
    }
}

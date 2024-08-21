using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XuongMay.Contract.Repositories.IUOW;
using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Repositories.UOW
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<OrderDetail> _orderDetails;

        public OrderDetailRepository(DatabaseContext context)
        {
            _context = context;
            _orderDetails = context.Set<OrderDetail>();
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await _orderDetails.ToListAsync();
        }

        public async Task<OrderDetail> GetByIdAsync(Guid id)
        {
            return await _orderDetails.FindAsync(id);
        }

        public async Task CreateAsync(OrderDetail orderDetail)
        {
            await _orderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Guid id, OrderDetail orderDetail)
        {
            var existingOrderDetail = await _orderDetails.FindAsync(id);
            if (existingOrderDetail == null) return false;

            _context.Entry(existingOrderDetail).CurrentValues.SetValues(orderDetail);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var orderDetail = await _orderDetails.FindAsync(id);
            if (orderDetail == null) return false;

            _orderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailByOrderIdAsync(Guid orderId)
        {
            return await _orderDetails.Where(od => od.OrderId == orderId).ToListAsync();
        }
    }
}

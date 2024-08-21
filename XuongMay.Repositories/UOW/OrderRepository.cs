using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XuongMay.Contract.Repositories.IUOW;
using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Repositories.UOW
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<Order> _orders;

        public OrderRepository(DatabaseContext context)
        {
            _context = context;
            _orders = context.Set<Order>();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _orders.ToListAsync();
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            return await _orders.FindAsync(id);
        }

        public async Task CreateAsync(Order order)
        {
            await _orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Guid id, Order order)
        {
            var existingOrder = await _orders.FindAsync(id);
            if (existingOrder == null) return false;

            _context.Entry(existingOrder).CurrentValues.SetValues(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var order = await _orders.FindAsync(id);
            if (order == null) return false;

            _orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

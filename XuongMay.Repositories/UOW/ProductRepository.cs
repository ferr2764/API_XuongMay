using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XuongMay.Contract.Repositories.IUOW;
using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Repositories.UOW
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<Product> _products;

        public ProductRepository(DatabaseContext context)
        {
            _context = context;
            _products = context.Set<Product>();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _products.FindAsync(id);
        }

        public async Task CreateAsync(Product product)
        {
            await _products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Guid id, Product product)
        {
            var existingProduct = await _products.FindAsync(id);
            if (existingProduct == null) return false;

            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _products.FindAsync(id);
            if (product == null) return false;

            _products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

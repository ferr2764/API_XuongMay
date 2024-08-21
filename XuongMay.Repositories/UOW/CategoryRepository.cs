using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Repositories.Interface;

namespace XuongMay.Repositories.UOW
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<Category> _categories;

        public CategoryRepository(DbContext context)
        {
            _context = context;
            _categories = context.Set<Category>();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            return await _categories.FindAsync(id);
        }

        public async Task InsertAsync(Category category)
        {
            await _categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Guid id, Category category)
        {
            var existingCategory = await _categories.FindAsync(id);
            if (existingCategory == null) return false;

            _context.Entry(existingCategory).CurrentValues.SetValues(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await _categories.FindAsync(id);
            if (category == null) return false;

            _categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

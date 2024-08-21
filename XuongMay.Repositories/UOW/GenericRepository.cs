using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XuongMay.Contract.Repositories.IUOW;
using XuongMay.Core;

namespace XuongMay.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> Entities => _dbSet.AsQueryable();

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T? GetById(Guid id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(T obj)
        {
            _dbSet.Add(obj);
            Save();
        }

        public void InsertRange(IList<T> obj)
        {
            _dbSet.AddRange(obj);
            Save();
        }

        public void Update(T obj)
        {
            _dbSet.Update(obj);
            Save();
        }

        public void Delete(Guid id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                Save();
            }
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<BasePaginatedList<T>> GetPaginatedAsync(IQueryable<T> query, int pageIndex, int pageSize)
        {
            var count = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new BasePaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task InsertAsync(T obj)
        {
            await _dbSet.AddAsync(obj);
            await SaveAsync();
        }

        public async Task InsertRangeAsync(IList<T> obj)
        {
            await _dbSet.AddRangeAsync(obj);
            await SaveAsync();
        }

        public async Task UpdateAsync(T obj)
        {
            _dbSet.Update(obj);
            await SaveAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await SaveAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

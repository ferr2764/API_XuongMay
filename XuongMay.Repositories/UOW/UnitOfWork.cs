using Microsoft.EntityFrameworkCore;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Repositories.Interface;
using XuongMay.Repositories.Context;

namespace XuongMay.Repositories.UOW
{
    public class UnitOfWork(DatabaseContext dbContext) : IUnitOfWork
    {
        private bool disposed = false;
        private readonly DatabaseContext _dbContext = dbContext;
        private IGenericRepository<Category> _categories;
        private IGenericRepository<Product> _products;
        private IGenericRepository<Order> _orders;
        private IGenericRepository<Account> _accounts;

        public IGenericRepository<Category> Categories => _categories ??= new GenericRepository<Category>(_dbContext);

        public IGenericRepository<Product> Products => _products ??= new GenericRepository<Product>(_dbContext);

        public IGenericRepository<Order> Orders => _orders ??= new GenericRepository<Order>(_dbContext);

        public IGenericRepository<Account> Accounts => _accounts ??= new GenericRepository<Account>(_dbContext);

        public void BeginTransaction()
        {
            _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _dbContext.Database.CommitTransaction();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }

        public void RollBack()
        {
            _dbContext.Database.RollbackTransaction();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_dbContext);
        }

        public async Task<int> CompleteAsync() => await _dbContext.SaveChangesAsync();

    }
}

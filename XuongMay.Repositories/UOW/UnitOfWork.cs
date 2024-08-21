using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XuongMay.Contract.Repositories.Interface;
using XuongMay.Contract.Repositories.IUOW;

namespace XuongMay.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private readonly ConcurrentDictionary<Type, object> _repositories;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<Type, object>();
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return (IGenericRepository<T>)_repositories.GetOrAdd(typeof(T), (type) => new GenericRepository<T>(_context));
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void BeginTransaction()
        {
        }

        public void CommitTransaction()
        {
        }

        public void RollBackTransaction()
        {
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

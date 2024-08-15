using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Contract.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        void Save();
        Task SaveAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<Account> Accounts { get; }
        Task<int> CompleteAsync();
    }
}

using MongoDB.Driver;
using System.Collections.Concurrent;
using XuongMay.Contract.Repositories.Interface;
using XuongMay.Contract.Repositories.IUOW;
using XuongMay.Repositories.UOW;

public class UnitOfWork : IUnitOfWork
{
    private readonly IMongoDatabase _database;
    private readonly IClientSessionHandle _session;
    private readonly ConcurrentDictionary<Type, object> _repositories;

    public UnitOfWork(IMongoClient mongoClient, IMongoDatabase database)
    {
        _database = database;
        _session = mongoClient.StartSession();
        _repositories = new ConcurrentDictionary<Type, object>();
    }

    public IGenericRepository<T> GetRepository<T>() where T : class
    {
        return (IGenericRepository<T>)_repositories.GetOrAdd(typeof(T), (type) => new GenericRepository<T>(_database));
    }

    public void Save()
    {
        _session.CommitTransaction();
    }

    public async Task SaveAsync()
    {
        await _session.CommitTransactionAsync();
    }

    public void BeginTransaction()
    {
        _session.StartTransaction();
    }

    public void CommitTransaction()
    {
        _session.CommitTransaction();
    }

    public void RollBackTransaction()
    {
        _session.AbortTransaction();
    }

    public void Dispose()
    {
        _session?.Dispose();
    }
}

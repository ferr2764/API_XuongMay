using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using XuongMay.Contract.Repositories.IUOW;
using XuongMay.Core;

namespace XuongMay.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;

        public GenericRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        public IQueryable<T> Entities => _collection.AsQueryable();

        public IEnumerable<T> GetAll()
        {
            return _collection.Find(_ => true).ToList();
        }

        public T? GetById(ObjectId id)
        {
            return _collection.Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefault();
        }

        public void Insert(T obj)
        {
            _collection.InsertOne(obj);
        }

        public void InsertRange(IList<T> obj)
        {
            _collection.InsertMany(obj);
        }

        public void Update(T obj)
        {
            var objectId = (ObjectId)obj.GetType().GetProperty("Id")?.GetValue(obj, null);
            _collection.ReplaceOne(Builders<T>.Filter.Eq("_id", objectId), obj);
        }

        public void Delete(ObjectId id)
        {
            _collection.DeleteOne(Builders<T>.Filter.Eq("_id", id));
        }

        public void Save()
        {
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<BasePaginatedList<T>> GetPaginatedAsync(IQueryable<T> query, int pageIndex, int pageSize)
        {
            var count = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new BasePaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public async Task<T?> GetByIdAsync(ObjectId id)
        {
            return await _collection.Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
        }

        public async Task InsertAsync(T obj)
        {
            await _collection.InsertOneAsync(obj);
        }

        public async Task InsertRangeAsync(IList<T> obj)
        {
            await _collection.InsertManyAsync(obj);
        }

        public async Task UpdateAsync(T obj)
        {
            var objectId = (ObjectId)obj.GetType().GetProperty("Id")?.GetValue(obj, null);
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", objectId), obj);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));
        }
   
        public async Task SaveAsync()
        {
            // MongoDB operations are immediately committed; no-op here.
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> GetAllByFilterAsync(FilterDefinition<T> filter)
        {
            return await _collection.Find(filter).ToListAsync();
        }
    }
}
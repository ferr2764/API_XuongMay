using MongoDB.Bson;
using MongoDB.Driver;
using XuongMay.Contract.Repositories.IUOW;
using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Repositories.UOW
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryRepository(IMongoDatabase database)
        {
            _categories = database.GetCollection<Category>("Categories");
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categories.Find(_ => true).ToListAsync();
        }

        public async Task<Category> GetByIdAsync(ObjectId id)
        {
            return await _categories.Find(category => category.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Category category)
        {
            await _categories.InsertOneAsync(category);
        }

        public async Task<bool> UpdateAsync(ObjectId id, Category category)
        {
            var result = await _categories.ReplaceOneAsync(c => c.Id == id, category);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(ObjectId id)
        {
            var result = await _categories.DeleteOneAsync(c => c.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
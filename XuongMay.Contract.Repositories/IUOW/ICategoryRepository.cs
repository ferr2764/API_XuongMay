using MongoDB.Bson;
using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Contract.Repositories.IUOW
{
    public interface ICategoryRepository
    {
        // Get all categories
        Task<IEnumerable<Category>> GetAllAsync();

        // Get category by Id
        Task<Category> GetByIdAsync(ObjectId id);

        // Create a new category
        Task CreateAsync(Category category);

        // Update an existing category
        Task<bool> UpdateAsync(ObjectId id, Category category);

        // Delete a category by Id
        Task<bool> DeleteAsync(ObjectId id);
    }
}

using MongoDB.Bson;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Repositories.Interface;
using XuongMay.Contract.Services.Interface;

namespace XuongMay.Services.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var repository = _unitOfWork.GetRepository<Category>();
            return await repository.GetAllAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<Category>();
            return await repository.GetByIdAsync(objectId);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            var repository = _unitOfWork.GetRepository<Category>();
            await repository.InsertAsync(category);
            //await _unitOfWork.SaveAsync();
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(string id, Category category)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<Category>();
            var existingCategory = await repository.GetByIdAsync(objectId);
            if (existingCategory == null)
                return null;

            // Update các thuộc tính cần thiết
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.CategoryDescription = category.CategoryDescription;
            existingCategory.CategoryStatus = category.CategoryStatus;

            repository.Update(existingCategory);
           // await _unitOfWork.SaveAsync();

            return existingCategory;
        }

        public async Task<bool> DeleteCategoryAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return false;

            var repository = _unitOfWork.GetRepository<Category>();
            var category = await repository.GetByIdAsync(objectId);
            if (category == null)
                return false;

            await repository.DeleteAsync(objectId);
            //await _unitOfWork.SaveAsync();

            return true;
        }
    }
}

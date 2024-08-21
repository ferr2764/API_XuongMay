using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Repositories.Interface;
using XuongMay.Contract.Services.Interface;
using XuongMay.ModelViews.CategoryModelViews;

namespace XuongMay.Services.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetPaginatedCategoriesAsync(int pageNumber, int pageSize)
        {
            var categories = await _categoryRepository.GetAllAsync();
            var pagedCategories = categories
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return pagedCategories;
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<Category> CreateCategoryAsync(CreateCategoryModelView categoryModelView)
        {
            var category = new Category
            {
                CategoryName = categoryModelView.CategoryName,
                CategoryDescription = categoryModelView.CategoryDescription,
            };

            await _categoryRepository.InsertAsync(category);
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Guid id, UpdateCategoryModelView categoryModelView)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null) return null;

            existingCategory.CategoryName = categoryModelView.CategoryName;
            existingCategory.CategoryDescription = categoryModelView.CategoryDescription;
            existingCategory.CategoryStatus = categoryModelView.CategoryStatus;

            await _categoryRepository.UpdateAsync(id, existingCategory);
            return existingCategory;
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null) return false;

            await _categoryRepository.DeleteAsync(id);
            return true;
        }
    }
}

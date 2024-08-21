using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.ModelViews.CategoryModelViews;

namespace XuongMay.Contract.Services.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetPaginatedCategoriesAsync(int pageNumber, int pageSize);
        Task<Category> GetCategoryByIdAsync(Guid id);
        Task<Category> CreateCategoryAsync(CreateCategoryModelView categoryModelView);
        Task<Category> UpdateCategoryAsync(Guid id, UpdateCategoryModelView categoryModelView);
        Task<bool> DeleteCategoryAsync(Guid id);
    }
}

using XuongMay.Contract.Repositories.Entity;
using XuongMay.ModelViews.CategoryModelViews;

namespace XuongMay.Contract.Services.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategoriesByPageAsync(int page, int pageSize);
        Task<Category> GetCategoryByIdAsync(string id);
        Task<Category> CreateCategoryAsync(CreateCategoryModelView categoryModelView);
        Task<Category> UpdateCategoryAsync(string id, UpdateCategoryModelView category);
        Task<bool> DeleteCategoryAsync(string id);
    }
}

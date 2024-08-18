using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.ModelViews.CategoryModelViews;

namespace XuongMay.Contract.Services.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(string id);
        Task<Category> CreateCategoryAsync(CreateCategoryModelView category);
        Task<Category> UpdateCategoryAsync(string id, Category category);
        Task<bool> DeleteCategoryAsync(string id);
    }
}

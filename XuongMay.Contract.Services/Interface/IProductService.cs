using XuongMay.Contract.Repositories.Entity;
using XuongMay.ModelViews.ProductModelViews;

namespace XuongMay.Contract.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetPaginatedProductsAsync(int pageNumber, int pageSize);
        Task<Product> GetProductByIdAsync(string id);
        Task<Product> CreateProductAsync(CreateProductModelView product);
        Task<Product> UpdateProductAsync(string id, UpdateProductModelView product);
        Task<bool> DeleteProductAsync(string id);
    }
}

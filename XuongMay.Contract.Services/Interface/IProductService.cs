using XuongMay.Contract.Repositories.Entity;
using XuongMay.ModelViews.ProductModelViews;

namespace XuongMay.Contract.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetPaginatedProductsAsync(int pageNumber, int pageSize);
        Task<Product> GetProductByIdAsync(Guid id);
        Task<Product> CreateProductAsync(CreateProductModelView createProduct);
        Task<Product> UpdateProductAsync(Guid id, UpdateProductModelView product);
        Task<bool> DeleteProductAsync(Guid id);
    }
}

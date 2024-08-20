using MongoDB.Bson;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Repositories.Interface;
using XuongMay.Contract.Services.Interface;
using XuongMay.ModelViews.ProductModelViews;

namespace XuongMay.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> GetPaginatedProductsAsync(int pageNumber, int pageSize)
        {
            var repository = _unitOfWork.GetRepository<Product>();
            var products = await repository.GetAllAsync();

            var pagedProducts = products
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            return pagedProducts;
        }


        public async Task<Product> GetProductByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<Product>();
            return await repository.GetByIdAsync(objectId);
        }

        public async Task<Product> CreateProductAsync(CreateProductModelView createProduct)
        {
            // Fetch the category from the repository
            var categoryRepository = _unitOfWork.GetRepository<Category>();
            var category = await categoryRepository.GetByIdAsync(ObjectId.Parse(createProduct.CategoryId));

            if (category == null)
            {
                throw new Exception("Category not found.");
            }

            if (category.CategoryStatus == "Unavailable")
            {
                throw new Exception("Cannot create a product in an unavailable category.");
            }

            Product product = new Product
            {
                ProductName = createProduct.ProductName,
                ProductSize = createProduct.ProductSize,
                Status = "Available",
                CategoryId = ObjectId.Parse(createProduct.CategoryId)
            };
            var repository = _unitOfWork.GetRepository<Product>();
            await repository.InsertAsync(product);
            //await _unitOfWork.SaveAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(string id, UpdateProductModelView product)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<Product>();
            var existingProduct = await repository.GetByIdAsync(objectId);
            if (existingProduct == null)
                return null;

            // Update các thuộc tính cần thiết
            existingProduct.ProductName = product.ProductName;
            existingProduct.ProductSize = product.ProductSize;
            existingProduct.Status = product.Status;
            existingProduct.CategoryId = ObjectId.Parse(product.CategoryId);

            repository.Update(existingProduct);
            //await _unitOfWork.SaveAsync();

            return existingProduct;
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return false;

            var repository = _unitOfWork.GetRepository<Product>();
            var existingProduct = await repository.GetByIdAsync(objectId);
            if (existingProduct == null)
                return false;

            // Update trạng thái thành Unavailable
            existingProduct.Status = "Unavailable";
           
            repository.Update(existingProduct);
            //await _unitOfWork.SaveAsync();

            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Repositories.Interface;
using XuongMay.Contract.Repositories.IUOW;
using XuongMay.Contract.Services.Interface;
using XuongMay.ModelViews.ProductModelViews;

namespace XuongMay.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> GetPaginatedProductsAsync(int pageNumber, int pageSize)
        {
            var products = await _productRepository.GetAllAsync();
            var pagedProducts = products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return pagedProducts;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<Product> CreateProductAsync(CreateProductModelView productModelView)
        {
            var product = new Product
            {
                ProductId = Guid.NewGuid(),
                ProductName = productModelView.ProductName,
                ProductSize = productModelView.ProductSize,
                CategoryId = productModelView.CategoryId
            };

            await _productRepository.CreateAsync(product);
            return product;
        }

        public async Task<Product> UpdateProductAsync(Guid id, UpdateProductModelView productModelView)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null) return null;

            existingProduct.ProductName = productModelView.ProductName;
            existingProduct.ProductSize = productModelView.ProductSize;

            await _productRepository.UpdateAsync(id, existingProduct);
            return existingProduct;
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null) return false;

            await _productRepository.DeleteAsync(id);
            return true;
        }
    }
}

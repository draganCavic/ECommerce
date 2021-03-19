using OnlineStore.DTOs;
using OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<IEnumerable<ProductDto>> GetUserProductsAsync(int userId);
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetProductByNameAsync(string name);
        void CreateProduct(ProductCreateDto product, int userId);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
    }
}

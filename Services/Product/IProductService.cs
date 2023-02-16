using store_be.Models;

namespace store_be.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProducts();

        Task<Product> AddProduct(Product productRequest);

        Task<Product> UpdateProduct(int id, Product productRequest);

        Task DeleteProduct(int id);

        Task<Product> GetProductById(int id);
    }
}

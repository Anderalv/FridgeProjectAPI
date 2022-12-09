using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Product;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        public Task<IEnumerable<ProductInFridgeDto>> GetProductsAsync(int fridgeId, bool trackChanges);
        
        public void CreateProduct(Product product);
        
        public Task<Product> GetProductAsync(int idProduct, bool trackChanges);
        
        public Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges);
        
        public Task<Product> GetProductByNameAsync(string name, bool trackChanges);
        
        public void DeleteProduct(Product product);
    }
}
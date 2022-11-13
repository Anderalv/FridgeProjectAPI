using System.Collections.Generic;
using Entities.DataTransferObjects;
using Entities.Models;

namespace Contracts
{
    public interface IProductRepository
    {
        IEnumerable<ProductInFridgeDto> GetProducts(int fridgeId, bool trackChanges);
        
        void CreateProduct(Product product);
        
        Product GetProduct(int idProduct, bool trackChanges);
        
        IEnumerable<Product> GetAllProducts(bool trackChanges);
        
        Product GetProductByName(string name, bool trackChanges);
        
        public void DeleteProduct(Product product);
    }
}
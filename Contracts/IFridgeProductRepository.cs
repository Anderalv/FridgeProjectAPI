using System.Collections.Generic;
using Entities.Models;

namespace Contracts
{
    public interface IFridgeProductRepository
    {
        void AddProductIntoFridge(FridgeProduct fridgeProduct);
        
        FridgeProduct GetFridgeProduct(int idProduct, int idFridge, bool trackChanges);
        
        void DeleteFridgeProduct(FridgeProduct fridgeProduct);
        
        public ICollection<FridgeProduct> GetFridgeProducts(bool trackChanges, int idFridge = 0, int idProduct = 0);
        
        public List<FridgeProduct> GetZeroFridgeProducts();
    }
}
using System.Collections.Generic;
using Entities.DataTransferObjects;
using Entities.Models;

namespace Contracts
{
    public interface IFridgeProductRepository
    {
        // IEnumerable<FridgeProduct> GetFridgeProducts(bool trackChanges);
        
        void AddProductIntoFridge(FridgeProduct fridgeProduct, bool trackChanges);
        FridgeProduct GetFridgeProduct(int idProduct, int idFridge, bool trackChanges);
        void DeleteProduct(FridgeProduct fridgeProduct);

        public List<FridgeProduct> Test();
    }
}
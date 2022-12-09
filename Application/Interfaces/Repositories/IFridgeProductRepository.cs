using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IFridgeProductRepository
    {
        public void AddProductIntoFridge(FridgeProduct fridgeProduct);
        
        public Task<FridgeProduct> GetFridgeProductAsync(int idProduct, int idFridge, bool trackChanges);
        
        public void DeleteFridgeProduct(FridgeProduct fridgeProduct);
        
        public Task<ICollection<FridgeProduct>> GetFridgeProductsAsync(bool trackChanges, int idFridge = 0, int idProduct = 0);
        
        public Task<List<FridgeProduct>> GetZeroFridgeProductsAsync();
    }
}
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;

namespace Repository
{
    public class FridgeProductRepository : RepositoryBase<FridgeProduct>, IFridgeProductRepository
    {
        public FridgeProductRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        // public IEnumerable<FridgeProduct> GetFridgeProducts(bool trackChanges) => 
        //     FindAll(trackChanges).ToList();
        public void AddProductIntoFridge(FridgeProduct fridgeProduct, bool trackChanges) => Create(fridgeProduct);

        public FridgeProduct GetFridgeProduct(int idProduct, int idFridge, bool trackChanges) => RepositoryContext.FridgeProducts
            .FirstOrDefault(x => x.IdFridge == idFridge && x.IdProduct == idProduct);
    }
}
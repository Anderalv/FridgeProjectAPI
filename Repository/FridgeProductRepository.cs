using System.Collections.Generic;
using System.Linq;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class FridgeProductRepository : RepositoryBase<FridgeProduct>, IFridgeProductRepository
    {
        private RepositoryContext _context;
        public FridgeProductRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        
        public void AddProductIntoFridge(FridgeProduct fridgeProduct) => Create(fridgeProduct);

        
        public FridgeProduct GetFridgeProduct(int idProduct, int idFridge, bool trackChanges) => RepositoryContext.FridgeProducts
            .FirstOrDefault(x => x.IdFridge == idFridge && x.IdProduct == idProduct);

        
        public ICollection<FridgeProduct> GetFridgeProducts(bool trackChanges, int idFridge = 0, int idProduct = 0)
        {
            if (idFridge != 0)
            {
                return RepositoryContext.FridgeProducts.Where(x => x.IdFridge == idFridge).ToList();
            }
            else
            {
                if (idProduct != 0) return RepositoryContext.FridgeProducts.Where(x => x.IdProduct == idProduct).ToList();
                else
                {
                    return null;
                }
            }
        }


        public void DeleteFridgeProduct(FridgeProduct fridgeProduct) => Delete(fridgeProduct);

        
        public List<FridgeProduct> GetZeroFridgeProducts()
        {
           return _context
                .FridgeProducts
                .FromSqlRaw("GetZeroFridgeProducts")
                .ToList();
        }
    }
}
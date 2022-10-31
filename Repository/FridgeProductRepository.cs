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

        // public IEnumerable<FridgeProduct> GetFridgeProducts(bool trackChanges) => 
        //     FindAll(trackChanges).ToList();
        public void AddProductIntoFridge(FridgeProduct fridgeProduct, bool trackChanges) => Create(fridgeProduct);

        public FridgeProduct GetFridgeProduct(int idProduct, int idFridge, bool trackChanges) => RepositoryContext.FridgeProducts
            .FirstOrDefault(x => x.IdFridge == idFridge && x.IdProduct == idProduct);

        public void DeleteProduct(FridgeProduct fridgeProduct) => Delete(fridgeProduct);

        public List<FridgeProduct> Test()
        {
           return _context
                .FridgeProducts
                .FromSqlRaw("GetZeroFridgeProducts")
                .ToList();
        }
    }
}
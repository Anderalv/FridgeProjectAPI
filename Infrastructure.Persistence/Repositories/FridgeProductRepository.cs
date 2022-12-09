using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class FridgeProductRepository : RepositoryBase<FridgeProduct>, IFridgeProductRepository
    {
        private RepositoryContext _context;
        public FridgeProductRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        
        public void AddProductIntoFridge(FridgeProduct fridgeProduct) => Create(fridgeProduct);

        
        public async Task<FridgeProduct> GetFridgeProductAsync(int idProduct, int idFridge, bool trackChanges) => await RepositoryContext.FridgeProducts
            .FirstOrDefaultAsync(x => x.IdFridge == idFridge && x.IdProduct == idProduct);

        
        public async Task<ICollection<FridgeProduct>> GetFridgeProductsAsync(bool trackChanges, int idFridge = 0, int idProduct = 0)
        {
            if (idFridge != 0)
            {
                return await RepositoryContext.FridgeProducts.Where(x => x.IdFridge == idFridge).ToListAsync();
            }
            else
            {
                if (idProduct != 0) return await RepositoryContext.FridgeProducts.Where(x => x.IdProduct == idProduct).ToListAsync();
                else
                {
                    return null;
                }
            }
        }


        public void DeleteFridgeProduct(FridgeProduct fridgeProduct) => Delete(fridgeProduct);

        
        public async Task<List<FridgeProduct>> GetZeroFridgeProductsAsync()
        {
           return await _context
                .FridgeProducts
                .FromSqlRaw("GetZeroFridgeProducts")
                .ToListAsync();
        }
    }
}
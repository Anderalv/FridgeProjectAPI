using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class FridgeRepository : RepositoryBase<Fridge>, IFridgeRepository
    {
        public FridgeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        
        
        public async Task<IEnumerable<Fridge>> GetAllFridgesAsync(bool trackChanges) => await FindAll(trackChanges)
            .OrderBy(c => c.Name) .ToListAsync();

      
        public async Task<Fridge> GetFridgeAsync(int fridgeId, bool trackChanges) => 
            await FindByCondition(c => c.Id.Equals(fridgeId), trackChanges).SingleOrDefaultAsync();
        
        
        public void DeleteFridge(Fridge fridge) => Delete(fridge);
        
        
        public void CreateFridge(Fridge fridge) => Create(fridge);
    }
}
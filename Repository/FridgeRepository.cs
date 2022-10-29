using System.Collections.Generic;
using System.Linq;
using Contracts;
using Entities.Models;

namespace Repository
{
    public class FridgeRepository : RepositoryBase<Fridge>, IFridgeRepository
    {
        public FridgeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        
        public IEnumerable<Fridge> GetAllFridges(bool trackChanges) => FindAll(trackChanges)
            .OrderBy(c => c.Name) .ToList();

      
        public Fridge GetFridge(int fridgeId, bool trackChanges) => 
            FindByCondition(c => c.Id.Equals(fridgeId), trackChanges).SingleOrDefault();
        
    }
}
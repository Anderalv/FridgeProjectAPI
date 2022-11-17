using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ModelRepository : RepositoryBase<Model>, IModelRepository
    {
        public ModelRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        
        public async Task<IEnumerable<Model>> GetAllFridgeModelsAsync(bool trackChanges)  => 
            await FindAll(trackChanges)
            .OrderBy(c => c.Name) .ToListAsync();

        
        public async Task<Model> GetModelAsync(int modelId, bool trackChanges) => 
                await FindByCondition(c => c.Id.Equals(modelId), trackChanges).SingleOrDefaultAsync();


        public async Task<Model> GetModelByNameAsync(string name, bool trackChanges) => 
            await FindByCondition(c => c.Name.Equals(name), trackChanges).SingleOrDefaultAsync();
    }
}
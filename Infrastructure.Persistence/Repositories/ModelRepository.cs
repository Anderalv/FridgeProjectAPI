using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
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
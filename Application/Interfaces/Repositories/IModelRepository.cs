using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IModelRepository
    {
        public Task<IEnumerable<Model>> GetAllFridgeModelsAsync(bool trackChanges);
        
        public Task<Model> GetModelAsync(int modelId, bool trackChanges);

        public Task<Model> GetModelByNameAsync(string name, bool trackChanges);
    }
}
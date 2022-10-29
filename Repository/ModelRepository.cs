using System.Collections.Generic;
using System.Linq;
using Contracts;
using Entities.Models;

namespace Repository
{
    public class ModelRepository : RepositoryBase<Model>, IModelRepository
    {
        public ModelRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Model> GetAllFridgeModels(bool trackChanges)  => FindAll(trackChanges)
            .OrderBy(c => c.Name) .ToList();
    }
}
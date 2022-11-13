using System.Collections.Generic;
using Entities.Models;

namespace Contracts
{
    public interface IModelRepository
    {
        IEnumerable<Model> GetAllFridgeModels(bool trackChanges);
        
        Model GetModel(int modelId, bool trackChanges);

        Model GetModelByName(string name, bool trackChanges);
    }
}
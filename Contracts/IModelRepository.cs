using System.Collections.Generic;
using Entities.Models;

namespace Contracts
{
    public interface IModelRepository
    {
        IEnumerable<Model> GetAllFridgeModels(bool trackChanges);
    }
}
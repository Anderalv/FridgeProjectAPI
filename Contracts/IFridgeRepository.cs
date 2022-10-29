using System.Collections.Generic;
using Entities.Models;

namespace Contracts
{
    public interface IFridgeRepository
    {
        IEnumerable<Fridge> GetAllFridges(bool trackChanges);
        Fridge GetFridge(int fridgeId, bool trackChanges);
    }
}
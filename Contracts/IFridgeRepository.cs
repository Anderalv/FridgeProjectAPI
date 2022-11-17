using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts
{
    public interface IFridgeRepository
    {
        public Task<IEnumerable<Fridge>> GetAllFridgesAsync(bool trackChanges);
        public Task<Fridge> GetFridgeAsync(int fridgeId, bool trackChanges);
        public void DeleteFridge(Fridge fridge);
        public void CreateFridge(Fridge fridge);
    }
}
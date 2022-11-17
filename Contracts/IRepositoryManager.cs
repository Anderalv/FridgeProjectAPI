using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryManager
    {
        IFridgeRepository Fridge { get; } 
        IModelRepository Model { get; }
        IProductRepository Product { get; }
        IFridgeProductRepository FridgeProduct { get; }
        Task SaveAsync();
    }
}
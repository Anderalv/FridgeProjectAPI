

namespace Contracts
{
    public interface IRepositoryManager
    {
        IFridgeRepository Fridge { get; } 
        IModelRepository Model { get; }
        IProductRepository Product { get; }
        IFridgeProductRepository FridgeProduct { get; }
        void Save();
    }
}
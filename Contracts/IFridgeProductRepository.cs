using System.Collections.Generic;
using Entities.Models;

namespace Contracts
{
    public interface IFridgeProductRepository
    {
        IEnumerable<FridgeProduct> GetFridgeProducts(bool trackChanges);
    }
}
using System.Collections.Generic;
using Entities.DataTransferObjects;
using Entities.Models;

namespace Contracts
{
    public interface IProductRepository
    {
        IEnumerable<ProductInFridgeDto> GetProducts(int fridgeId, bool trackChanges);
    }
}
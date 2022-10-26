using System.Collections.Generic;
using Entities.Models;

namespace Contracts
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts(bool trackChanges);
    }
}
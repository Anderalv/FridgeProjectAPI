using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;

namespace Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        
        public IEnumerable<ProductInFridgeDto>GetProducts(int fridgeId, bool trackChanges)
        {
            return RepositoryContext.FridgeProducts
                .Where(x => x.IdFridge == fridgeId)
                .Select(x => new ProductInFridgeDto
                {
                    Id = x.IdProduct,
                    Name = x.Product.Name,
                    Quantity = x.Quantity
                });
        }
    }
}
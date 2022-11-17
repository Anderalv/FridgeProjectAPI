using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        
        
        public async Task<IEnumerable<ProductInFridgeDto>> GetProductsAsync(int fridgeId, bool trackChanges)
        {
            return await RepositoryContext.FridgeProducts
                .Where(x => x.IdFridge == fridgeId)
                .Select(x => new ProductInFridgeDto
                {
                    Id = x.IdProduct,
                    Name = x.Product.Name,
                    Quantity = x.Quantity
                }).ToListAsync();
        }

        
        public void CreateProduct(Product product) => Create(product);
        
        
        public async Task<Product> GetProductAsync(int productId, bool trackChanges) =>  
            await FindByCondition(c => c.Id.Equals(productId), trackChanges).SingleOrDefaultAsync();
        

        public async Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges) => 
            await FindAll(trackChanges)
            .OrderBy(c => c.Name) .ToListAsync();

        
        public async Task<Product> GetProductByNameAsync(string name, bool trackChanges) =>  
            await FindByCondition(c => c.Name.Equals(name), trackChanges).SingleOrDefaultAsync();

        
        public void DeleteProduct(Product product) => Delete(product);
    }
}
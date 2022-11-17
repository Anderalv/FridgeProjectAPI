using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace FridgeProject.Controllers
{
    [Route("api/products")] 
    [ApiController]
    public class ProductController : ControllerBase {
        
        private readonly IRepositoryManager _repository; 
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        
        public ProductController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository; _logger = logger;
            _mapper = mapper;
        } 
        
        
        [HttpGet("{id}", Name = "ProductById")]
        public async Task<IActionResult> GetProduct(int id) 
        {
            var product = await _repository.Product.GetProductAsync(id, trackChanges: false); 
            if(product == null)
            { 
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database."); 
                return NotFound(); 
            }

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                DefaultQuantity = product.DefaultQuantity
            };
            return Ok(productDto);
        }

        
        [HttpPost("CreateProduct")]
         public async Task<IActionResult> CreateProduct([FromBody] ProductForCreateDto product)
         {
             if (product == null)
             {
                 _logger.LogError("ProductForCreateDto object sent from client is null.");
                 return BadRequest("ProductForCreateDto object is null");
             }

             if (!ModelState.IsValid) {
                 _logger.LogError("Invalid model state for the ProductForCreateDto object"); 
                 return UnprocessableEntity(ModelState); 
             }

             var productEntity = new Product
             {
                 Name = product.Name,
                 DefaultQuantity = product.Quantity
             };
             _repository.Product.CreateProduct(productEntity);
             await _repository.SaveAsync();
             var productToReturn = new ProductDto
             {
                 Name = product.Name,
                 DefaultQuantity = product.Quantity
             };
             return CreatedAtRoute("ProductById", new { id = productToReturn.Id }, productToReturn);
         }
         
         
         [HttpPut("{productId}")]
         public async Task<IActionResult> UpdateProduct(int productId, [FromBody] ProductForUpdateDto product)
         {
             if(product == null) 
             {
                 _logger.LogError("product object sent from client is null.");
                 return BadRequest("product object is null"); 
             }

             if (!ModelState.IsValid) 
             {
                 _logger.LogError("Invalid model state for the UpdateProduct object"); 
                 return UnprocessableEntity(ModelState); 
             }

             var productEntity = await _repository.Product.GetProductAsync(productId, true);
             if(productEntity == null) 
             {
                 _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                 return NotFound(); 
             }

             productEntity.Name = product.Name;
             productEntity.DefaultQuantity = product.Quantity;
             await _repository.SaveAsync();
             return Ok(productEntity);
         }
         
         
         [HttpGet("AllProducts")]
         public async Task<IActionResult> GetAllProducts()
         {
             var products = await _repository.Product.GetAllProductsAsync( trackChanges: false);
             return Ok(products);
         }
         
         
         [HttpDelete("DeleteProduct/{idProduct}")]
         public async Task<IActionResult> DeleteProduct(int idProduct) 
         {
             var product = await _repository.Product.GetProductAsync(idProduct, false);
             if(product == null)
             {
                 _logger.LogInfo($"error of input data");
                 return NotFound(); 
             }
            
             var fridgeProducts = await _repository.FridgeProduct
                 .GetFridgeProductsAsync(false, idProduct: idProduct);

             foreach (var fridgeProduct in fridgeProducts)
             {
                 _repository.FridgeProduct.DeleteFridgeProduct(fridgeProduct);
             }
             _repository.Product.DeleteProduct(product);
             await _repository.SaveAsync();
             return NoContent();
         }
    }
}


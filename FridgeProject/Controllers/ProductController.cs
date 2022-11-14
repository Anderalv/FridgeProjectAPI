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
        public IActionResult GetProduct(int id) 
        {
            var product = _repository.Product.GetProduct(id, trackChanges: false); 
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
         public IActionResult CreateProduct([FromBody] ProductForCreateDto product)
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
             _repository.Save();
             var productToReturn = new ProductDto
             {
                 Name = product.Name,
                 DefaultQuantity = product.Quantity
             };
             return CreatedAtRoute("ProductById", new { id = productToReturn.Id }, productToReturn);
         }
         
         
         [HttpPut("{productId}")]
         public IActionResult UpdateProduct(int productId, [FromBody] ProductForUpdateDto product)
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

             var productEntity = _repository.Product.GetProduct(productId, true);
             if(productEntity == null) 
             {
                 _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                 return NotFound(); 
             }

             productEntity.Name = product.Name;
             productEntity.DefaultQuantity = product.Quantity;
             _repository.Save();
             return Ok(productEntity);
         }
         
         
         [HttpGet("AllProducts")]
         public IActionResult GetAllProducts()
         {
             var products = _repository.Product.GetAllProducts( trackChanges: false);
             return Ok(products);
         }
         
         
         [HttpDelete("DeleteProduct/{idProduct}")]
         public IActionResult DeleteProduct(int idProduct) 
         {
             var product = _repository.Product.GetProduct(idProduct, false);
             if(product == null)
             {
                 _logger.LogInfo($"error of input data");
                 return NotFound(); 
             }
            
             var fridgeProducts = _repository.FridgeProduct
                 .GetFridgeProducts(false, idProduct: idProduct);

             foreach (var fridgeProduct in fridgeProducts)
             {
                 _repository.FridgeProduct.DeleteFridgeProduct(fridgeProduct);
             }
             _repository.Product.DeleteProduct(product);
             _repository.Save();
             return NoContent();
         }
    }
}


using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repository;

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
        public IActionResult GetProduct(int id) {
            var product = _repository.Product.GetProduct(id, trackChanges: false); 
            if(product == null)
            { 
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database."); 
                return NotFound(); 
            }
            else
            {
                var productDto = _mapper.Map<ProductDto>(product);
                return Ok(productDto);
            } 
        }

        [HttpPost]
         public IActionResult CreateProduct([FromBody] ProductForCreationDto product)
         {
             if (product == null)
             {
                 _logger.LogError("ProductForCreationDto object sent from client is null.");
                 return BadRequest("ProductForCreationDto object is null");
             }

             if (!ModelState.IsValid) {
                 _logger.LogError("Invalid model state for the ProductForCreationDto object"); 
                 return UnprocessableEntity(ModelState); 
             }
             
             var productEntity = _mapper.Map<Product>(product);
             _repository.Product.CreateProduct(productEntity);
             _repository.Save();
             var productToReturn = _mapper.Map<ProductDto>(productEntity);
             return CreatedAtRoute("ProductById", new { id = productToReturn.Id }, productToReturn);
         }
         
         [HttpPut("{productId}")]
         public IActionResult UpdateProduct(int productId, [FromBody] ProductForUpdateDto product)
         {
             if(product == null) {
                 _logger.LogError("product object sent from client is null.");
                 return BadRequest("product object is null"); }
             
             
             
             if (!ModelState.IsValid) {
                 _logger.LogError("Invalid model state for the UpdateProduct object"); 
                 return UnprocessableEntity(ModelState); 
             }
             
             
             
             
             var productEntity = _repository.Product.GetProduct(productId, true);
             if(productEntity == null) {
                 _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                 return NotFound(); 
             }
             else
             {
                 productEntity.Name = product.Name;
                 productEntity.DefaultQuantity = product.DefaultQuantity;
                 _repository.Save();
                 return NoContent(); 
             }
         }
    }
}


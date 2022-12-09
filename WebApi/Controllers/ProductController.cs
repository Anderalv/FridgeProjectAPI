using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Behaviours.ActionFilters;
using Application.DTOs.Product;
using AutoMapper;
using Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/products")] 
    [ApiController]
    public class ProductController : ControllerBase {
        
        private readonly IRepositoryManager _repository; 
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        
        public ProductController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository; 
            _logger = logger;
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
            return Ok(_mapper.Map<ProductDto>(product));
        }

       
        [HttpPost("CreateProduct")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
         public async Task<IActionResult> CreateProduct([FromBody] ProductForCreateDto product)
         {
             var productEntity = _mapper.Map<Product>(product); 
             _repository.Product.CreateProduct(productEntity);
             await _repository.SaveAsync();
             var productToReturn = _mapper.Map<ProductDto>(productEntity);
             return CreatedAtRoute("ProductById", new { id = productToReturn.Id }, productToReturn);
         }
         
         
         [HttpPut("{productId}")]
         [ServiceFilter(typeof(ValidationFilterAttribute))]
         public async Task<IActionResult> UpdateProduct(int productId, [FromBody] ProductForUpdateDto product)
         {
             var productEntity = await _repository.Product.GetProductAsync(productId, true);
             if(productEntity == null) 
             {
                 _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                 return NotFound(); 
             }

             _mapper.Map(product, productEntity);
             await _repository.SaveAsync();
             
             return NoContent();
         }
         
         
         [HttpGet("AllProducts")]
         public async Task<IActionResult> GetAllProducts()
         {
             var products = await _repository.Product.GetAllProductsAsync( trackChanges: false);
             var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
             return Ok(productsDto);
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
             
             _repository.Product.DeleteProduct(product);
             await _repository.SaveAsync();
             return NoContent();
         }
    }
}
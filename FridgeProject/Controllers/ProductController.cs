// using System.Collections.Generic;
// using System.Linq;
// using AutoMapper;
// using Contracts;
// using Entities.DataTransferObjects;
// using Entities.Models;
// using Microsoft.AspNetCore.Mvc;
// using Repository;
//
// namespace FridgeProject.Controllers
// {
//     [Route("api/fridge/products")]
//     [ApiController]
//     public class ProductController : ControllerBase
//     {
//
//         private readonly IRepositoryManager _repository;
//         private readonly ILoggerManager _logger;
//         private readonly IMapper _mapper;
//
//         public ProductController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
//         {
//             _repository = repository;
//             _logger = logger;
//             _mapper = mapper;
//         }
//         
//         
//         [HttpGet("{id}", Name = "ProductById")]
//         public IActionResult GetCompany(int id) {
//             var product = _repository.Fridge.GetFridge(id, trackChanges: false); 
//             if(product == null)
//             {
//                 _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
//                 return NotFound(); 
//             }
//             else
//             {
//                 var productDto = _mapper.Map<ProductDto>(product);
//                 return Ok(productDto);
//             } 
//         }
//         
//
//         
//         [HttpGet("{fridgeId}")]
//         public IActionResult GetProductsInTheFridge(int fridgeId)
//         {
//             var fridge = _repository.Fridge.GetFridge(fridgeId, trackChanges: false);
//         
//             if (fridge == null)
//             {
//                 _logger.LogInfo($"Fridge with id: {fridgeId} doesn't exist in the database.");
//                 return NotFound();
//             }
//         
//             var productsFromDb = _repository.Product.GetProducts(fridgeId, trackChanges: false);
//             return Ok(productsFromDb);
//         }
//
//         [HttpPost]
//         public IActionResult CreateProduct([FromBody] ProductForCreationDto product)
//         {
//             if (product == null)
//             {
//                 _logger.LogError("ProductForCreationDto object sent from client is null.");
//                 return BadRequest("ProductForCreationDto object is null");
//             }
//
//             var productEntity = _mapper.Map<Product>(product);
//             _repository.Product.CreateProduct(productEntity);
//             _repository.Save();
//             var productToReturn = _mapper.Map<ProductOnFridgeDto>(productEntity);
//             return CreatedAtRoute("ProductById", new { id = productToReturn.Id }, productToReturn);
//         }
//     }
// }

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

             var productEntity = _mapper.Map<Product>(product);
             _repository.Product.CreateProduct(productEntity);
             _repository.Save();
             var productToReturn = _mapper.Map<ProductDto>(productEntity);
             return CreatedAtRoute("ProductById", new { id = productToReturn.Id }, productToReturn);
         }
    }
}


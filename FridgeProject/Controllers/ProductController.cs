using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace FridgeProject.Controllers
{
    [Route("api/fridge/{fridgeId}/products")] 
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
        
        [HttpGet]
        public IActionResult GetProductsInTheFridge(int fridgeId) {
            var fridge = _repository.Fridge.GetFridge(fridgeId, trackChanges: false); 
            
            if(fridge == null)
            {
                _logger.LogInfo($"Fridge with id: {fridgeId} doesn't exist in the database.");
                return NotFound(); 
            }
            
            var productsFromDb = _repository.Product.GetProducts(fridgeId, trackChanges: false);
            return Ok(productsFromDb);
        }
    }
}
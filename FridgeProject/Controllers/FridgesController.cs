using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace FridgeProject.Controllers
{
    [Route("api/Fridges")]
    [ApiController]
    public class FridgesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public FridgesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetFridges()
        {
            var fridges = _repository.Fridge.GetAllFridges(trackChanges: false);
            var fridgesDto = _mapper.Map<IEnumerable<FridgeDto>>(fridges);
            return Ok(fridgesDto);
        }
        
        [HttpGet("{fridgeId}", Name = "ProductsOnFridge")]
        public IActionResult GetProductsFromTheFridge(int fridgeId) {
            var fridge = _repository.Fridge.GetFridge(fridgeId, trackChanges: false); 
            
            if(fridge == null)
            {
                _logger.LogInfo($"Fridge with id: {fridgeId} doesn't exist in the database.");
                return NotFound(); 
            }
            
            var productsFromDb = _repository.Product.GetProducts(fridgeId, trackChanges: false);
            return Ok(productsFromDb);
        }
        
        
        
        [HttpPost("AddProductInTheFridge")]
        public IActionResult AddProductInTheFridge([FromBody] ProductForAddToFridgeDto productForAddToFridgeDto) {
            
            
            if(productForAddToFridgeDto == null) {
                _logger.LogError("Product for add to fridge sent from client is null.");
                return BadRequest("Product for add to fridge is null"); 
            }

            if (_repository.Product.GetProductByName(productForAddToFridgeDto.Name, false) == null)
            {
                _logger.LogError("No product with this name on db.");
                return BadRequest("No product with this name on db. You should add this product on db and try again");
            }
            
            if (!ModelState.IsValid) {
                _logger.LogError("Invalid model state for the ProductForAddToFridgeDto object"); 
                return UnprocessableEntity(ModelState); 
            }

            var productId = _repository.Product.GetProductByName(productForAddToFridgeDto.Name,
                false).Id;

            if (_repository.FridgeProduct.GetFridgeProduct(productId, productForAddToFridgeDto.FridgeId, false) != null)
            {
                var newStr = _repository.FridgeProduct.GetFridgeProduct(productId, productForAddToFridgeDto.FridgeId, true);
                
                var quantity = _repository.FridgeProduct.GetFridgeProduct(productId, productForAddToFridgeDto.FridgeId, false)
                    .Quantity + productForAddToFridgeDto.Quantity;

                newStr.Quantity = quantity;
                _repository.Save();
                
                var productsFromDb = _repository.Product.GetProducts(productForAddToFridgeDto.FridgeId, trackChanges: false);
            
                return CreatedAtRoute("ProductsOnFridge", new { fridgeId =productForAddToFridgeDto.FridgeId}, productsFromDb);
            }
            else
            {
                FridgeProduct fridgeProduct = new FridgeProduct
                {
                    IdProduct = productId,
                    IdFridge = productForAddToFridgeDto.FridgeId,
                    Quantity = productForAddToFridgeDto.Quantity
                };

                _repository.FridgeProduct.AddProductIntoFridge(fridgeProduct, false);
                _repository.Save();
                var productsFromDb = _repository.Product.GetProducts(productForAddToFridgeDto.FridgeId, trackChanges: false);

                return CreatedAtRoute("ProductsOnFridge", new { fridgeId =productForAddToFridgeDto.FridgeId}, productsFromDb);
            }
        }
        
        [HttpDelete("DeleteProduct")]
        public IActionResult DeleteProductFromFridge([FromBody] DeleteProductDto deleteProduct) {

            var fridge = _repository.Fridge.GetFridge(deleteProduct.FridgeId, trackChanges: false);
            var product = _repository.Product.GetProductByName(deleteProduct.Name, false);
            
            if(fridge == null || product == null)
            {
                _logger.LogInfo($"error of input data");
                return NotFound(); 
            }
            
            if (!ModelState.IsValid) {
                _logger.LogError("Invalid model state for the DeleteProductFromFridge object"); 
                return UnprocessableEntity(ModelState); 
            }

            var fridgeProduct = _repository.FridgeProduct.GetFridgeProduct(product.Id, fridge.Id, true);

            if (fridgeProduct.Quantity <= deleteProduct.Quantity)
            {
                _repository.FridgeProduct.DeleteProduct(fridgeProduct);
                _repository.Save();
            }
            else
            {
                fridgeProduct.Quantity = fridgeProduct.Quantity - deleteProduct.Quantity;
                _repository.Save();
            }
            return NoContent(); }
    }
}
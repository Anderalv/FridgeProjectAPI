using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public FridgesController(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
        
        
        [HttpGet]
        public async Task<IActionResult> GetFridges()
        {
            List<FridgeWithModelDto> fridgeWithModelDtos = new List<FridgeWithModelDto>();
            var fridges = await _repository.Fridge.GetAllFridgesAsync(trackChanges: false);
            foreach (var fridge in fridges)
            {
                var model = await _repository.Model.GetModelAsync(fridge.IdModel, false);
                var fridgeWithModelDto = new FridgeWithModelDto
                {
                    IdFridge = fridge.Id,
                    Name = fridge.Name,
                    OwnerName = fridge.OwnerName,
                    IdModel = fridge.IdModel,
                    ModelName = model.Name,
                    Year = model.Year
                };
                fridgeWithModelDtos.Add(fridgeWithModelDto);
            }
            
            return Ok(fridgeWithModelDtos);
        }
        
        
        [HttpGet("/ProductOnFridge/{fridgeId}", Name = "ProductsOnFridge")]
        public async Task<IActionResult> GetProductsFromTheFridge(int fridgeId) 
        {
            var fridge = await _repository.Fridge.GetFridgeAsync(fridgeId, trackChanges: false); 
            
            if(fridge == null)
            {
                _logger.LogInfo($"Fridge with id: {fridgeId} doesn't exist in the database.");
                return NotFound(); 
            }
            
            var productsFromDb = await _repository.Product.GetProductsAsync(fridgeId, trackChanges: false);
            return Ok(productsFromDb);
        }
        
        
        [HttpPost("/AddProductInTheFridge/{fridgeId}", Name ="AddProductInTheFridge")]
        public async Task<IActionResult> AddProductInTheFridge(int fridgeId,[FromBody] ProductForAddToFridgeDto productForAddToFridgeDto) 
        {
            if(productForAddToFridgeDto == null) 
            {
                _logger.LogError("Product for add to fridge sent from client is null.");
                return BadRequest("Product for add to fridge is null"); 
            }

            if (await _repository.Product.GetProductByNameAsync(productForAddToFridgeDto.Name, false) == null)
            {
                _logger.LogError("No product with this name on db.");
                return BadRequest("No product with this name on db. You should add this product on db and try again");
            }
            
            if (!ModelState.IsValid) 
            {
                _logger.LogError("Invalid model state for the ProductForAddToFridgeDto object"); 
                return UnprocessableEntity(ModelState); 
            }
//////??????????????????????? await
            var productId = (await _repository.Product.GetProductByNameAsync(productForAddToFridgeDto.Name,
                false)).Id;

            if (await _repository.FridgeProduct.GetFridgeProductAsync(productId, productForAddToFridgeDto.FridgeId, false) != null)
            {
                var newStr = await _repository.FridgeProduct.GetFridgeProductAsync(productId, productForAddToFridgeDto.FridgeId, true); 
//////??????????????????????? await
                var quantity = (await _repository.FridgeProduct.GetFridgeProductAsync(productId, productForAddToFridgeDto.FridgeId, false))
                    .Quantity + productForAddToFridgeDto.Quantity;

                newStr.Quantity = quantity;
                await _repository.SaveAsync();
                
                var productsFromDb = await _repository.Product.GetProductsAsync(productForAddToFridgeDto.FridgeId, trackChanges: false);
                return Ok(productsFromDb);
            }
            else
            {
                FridgeProduct fridgeProduct = new FridgeProduct
                {
                    IdProduct = productId,
                    IdFridge = productForAddToFridgeDto.FridgeId,
                    Quantity = productForAddToFridgeDto.Quantity
                };

                _repository.FridgeProduct.AddProductIntoFridge(fridgeProduct);
                await  _repository.SaveAsync();
                var productsFromDb = await _repository.Product.GetProductsAsync(productForAddToFridgeDto.FridgeId, trackChanges: false);
                return Ok(productsFromDb);
            }
        }
        
        
        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProductFromFridge([FromBody] DeleteProductDto deleteProduct) 
        {
            var fridge = await _repository.Fridge.GetFridgeAsync(deleteProduct.FridgeId, trackChanges: false);
            var product = await _repository.Product.GetProductByNameAsync(deleteProduct.Name, false);
            
            if(fridge == null || product == null)
            {
                _logger.LogInfo($"error of input data");
                return NotFound(); 
            }
            
            if (!ModelState.IsValid) 
            {
                _logger.LogError("Invalid model state for the DeleteProductFromFridge object"); 
                return UnprocessableEntity(ModelState); 
            }

            var fridgeProduct = await _repository.FridgeProduct.GetFridgeProductAsync(product.Id, fridge.Id, true);

            if (fridgeProduct.Quantity <= deleteProduct.Quantity)
            {
                _repository.FridgeProduct.DeleteFridgeProduct(fridgeProduct);
            }
            else
            {
                fridgeProduct.Quantity -= deleteProduct.Quantity;
            }
            await _repository.SaveAsync();
            return NoContent();
        }

        
        [HttpPut("CallStoredProcedure")]
        public async void CallStoredProcedure()
        {
            var badProducts = await _repository.FridgeProduct.GetZeroFridgeProductsAsync();

            foreach (var fridgeProduct in badProducts)
            {
                var product = await _repository.Product.GetProductAsync(fridgeProduct.IdProduct, false);
                ProductForAddToFridgeDto productForAddToFridgeDto = new ProductForAddToFridgeDto
                {
                    FridgeId = fridgeProduct.IdFridge,
                    Name = product.Name,
                    Quantity = product.DefaultQuantity
                };

                await AddProductInTheFridge(fridgeProduct.IdFridge, productForAddToFridgeDto);
            }
//////////////
            foreach (var fridgeProduct in badProducts)
            {
                var rowForChange = await _repository.FridgeProduct.GetFridgeProductAsync(fridgeProduct.IdProduct
                    , fridgeProduct.IdFridge, true);
         
                var defaultQuality = (await _repository.Product.GetProductAsync(fridgeProduct.IdProduct, false))
                    .DefaultQuantity;
            
                rowForChange.Quantity = defaultQuality;
                await _repository.SaveAsync();
            }
//////////////            
        }

        
        [HttpPut("/EditFridge/{idFridge}")]
        public async Task<IActionResult> EditFridge(int idFridge,[FromBody] EditFridgeDto editFridgeDto)
        {
            var editFridge = await _repository.Fridge.GetFridgeAsync(idFridge, true);
            if (string.IsNullOrEmpty(editFridgeDto.Name) || string.IsNullOrEmpty(editFridgeDto.OwnerName))
            {
                _logger.LogInfo($"write old or new name/owner name");
                return NotFound(); 
            }

            editFridge.Name = editFridgeDto.Name;
            editFridge.OwnerName = editFridgeDto.OwnerName;
            editFridge.IdModel = editFridgeDto.IdModel;
            await _repository.SaveAsync();

            foreach (var editedProduct in editFridgeDto.EditedProducts)
            {
                if (editedProduct.EditedQuantity == 0)
                {
                    var productOnFridge = await _repository.FridgeProduct.GetFridgeProductAsync(editedProduct.IdProduct, idFridge, false);
                    _repository.FridgeProduct.DeleteFridgeProduct(productOnFridge);
                }
                else
                {
                    var productOnFridge = await _repository.FridgeProduct.GetFridgeProductAsync(editedProduct.IdProduct, idFridge, true);
                    productOnFridge.Quantity = editedProduct.EditedQuantity;
                }
                await _repository.SaveAsync();
            }

            foreach (var idAddedProduct in editFridgeDto.AddedProducts)
            {
                var newProductForFridge = await _repository.Product.GetProductAsync(idAddedProduct, false);
                var fridgeProduct = new FridgeProduct
                {
                    IdProduct = idAddedProduct,
                    IdFridge = idFridge,
                    Quantity = newProductForFridge.DefaultQuantity
                };
                _repository.FridgeProduct.AddProductIntoFridge(fridgeProduct);
                await _repository.SaveAsync();
            }
            
            return NoContent();
        }
        
        
        [HttpDelete("DeleteFridge/{idFridge}")]
        public async Task<IActionResult> DeleteFridge(int idFridge) 
        {
            var fridge = await _repository.Fridge.GetFridgeAsync(idFridge, trackChanges: false);
            
            if(fridge == null)
            {
                _logger.LogInfo($"error of input data");
                return NotFound(); 
            }
            
            var fridgeProducts = await _repository.FridgeProduct.GetFridgeProductsAsync(false, idFridge: idFridge);

            foreach (var fridgeProduct in fridgeProducts)
            {
                _repository.FridgeProduct.DeleteFridgeProduct(fridgeProduct);
            }
            _repository.Fridge.DeleteFridge(fridge);
            await _repository.SaveAsync();
            return NoContent();
        }
        
        
        [HttpPost("CreateFridge")]
        public async Task<IActionResult> CreateFridge([FromBody] FridgeForCreationDto fridge)
        {
            if (fridge == null)
            {
                _logger.LogError("FridgeForCreationDto object sent from client is null.");
                return BadRequest("FridgeForCreationDto object is null");
            }

            if (!ModelState.IsValid) 
            {
                _logger.LogError("Invalid model state for the FridgeForCreationDto object"); 
                return UnprocessableEntity(ModelState); 
            }

            var newFridge = new Fridge
            {
                Name = fridge.Name,
                OwnerName = fridge.OwnerName,
                IdModel = fridge.IdModel,
            };

            _repository.Fridge.CreateFridge(newFridge);
            await _repository.SaveAsync();
            
            var idNewFridge = (await _repository.Fridge.GetAllFridgesAsync(false)).OrderBy(x => x.Id).Last().Id;

            foreach (var item in fridge.ProductsId)
            {
                var product = await _repository.Product.GetProductAsync(item, false);

                var fridgeProduct = new FridgeProduct
                {
                    IdProduct = item,
                    IdFridge = idNewFridge,
                    Quantity = product.DefaultQuantity
                    
                    //for test Stored Procedure
                    // Quantity = 0
                };
                
                _repository.FridgeProduct.AddProductIntoFridge(fridgeProduct);
            }
            
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
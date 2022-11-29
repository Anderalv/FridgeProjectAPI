using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using FridgeProject.ActionFilters;
using Microsoft.AspNetCore.Authorization;
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
        
      
        [HttpGet, Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetFridges()
        {
            var fridgeWithModelDtos = new List<FridgeWithModelDto>();
            
            var fridges = await _repository.Fridge.GetAllFridgesAsync(trackChanges: false);
            var fridgesDto = _mapper.Map<IEnumerable<FridgeDto>>(fridges);
            
            foreach (var fridge in fridgesDto)
            {
                var model = await _repository.Model.GetModelAsync(fridge.IdModel, false);
                var modelDto = _mapper.Map<ModelDto>(model);
                
                var fridgeWithModelDto = new FridgeWithModelDto
                {
                    IdFridge = fridge.Id,
                    Name = fridge.Name,
                    OwnerName = fridge.OwnerName,
                    IdModel = fridge.IdModel,
                    ModelName = modelDto.Name,
                    Year = modelDto.Year
                };
                
                fridgeWithModelDtos.Add(fridgeWithModelDto);
            }
            return Ok(fridgeWithModelDtos);
        }


        [HttpGet("/ProductOnFridge/{fridgeId}", Name = "ProductsOnFridge")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetProductsFromTheFridge(int fridgeId) 
        {
            if(await _repository.Fridge.GetFridgeAsync(fridgeId, trackChanges: false) == null)
            {
                _logger.LogInfo($"Fridge with id: {fridgeId} doesn't exist in the database.");
                return NotFound(); 
            }
            
            var productsFromDb = await _repository.Product.GetProductsAsync(fridgeId, trackChanges: false);
            var fridgeProductsToReturn = _mapper.Map<IEnumerable<ProductInFridgeDtoToReturn>>(productsFromDb);
            
            return Ok(fridgeProductsToReturn);
        }

        
        [HttpPost("/AddProductInTheFridge/{fridgeId}", Name ="AddProductInTheFridge")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddProductInTheFridge(int fridgeId,[FromBody] ProductForAddToFridgeDto productForAddToFridgeDto)
        {
            var product = await _repository.Product.GetProductByNameAsync(productForAddToFridgeDto.Name, false);
            if (product == null)
            {
                _logger.LogError("No product with this name on db.");
                return BadRequest("No product with this name on db. You should add this product on db and try again");
            }

            var productId = product.Id;

            var fridgeProduct = await _repository.FridgeProduct.GetFridgeProductAsync(productId, productForAddToFridgeDto.FridgeId, true); 
            if (fridgeProduct != null)
            { 
                fridgeProduct.Quantity += productForAddToFridgeDto.Quantity;
                await _repository.SaveAsync();
                
                var productsFromDb = await _repository.Product.GetProductsAsync(productForAddToFridgeDto.FridgeId, trackChanges: false);
                var productsToReturn = _mapper.Map<IEnumerable<ProductInFridgeDtoToReturn>>(productsFromDb);
                return Ok(productsToReturn);
            }
            else
            {
                var newFridgeProduct = _mapper.Map<FridgeProduct>(productForAddToFridgeDto);
                newFridgeProduct.IdProduct = productId;
                
                _repository.FridgeProduct.AddProductIntoFridge(newFridgeProduct);
                await  _repository.SaveAsync();
                var productsFromDb = await _repository.Product.GetProductsAsync(productForAddToFridgeDto.FridgeId, trackChanges: false);
                var productsToReturn = _mapper.Map<IEnumerable<ProductInFridgeDtoToReturn>>(productsFromDb);
                return Ok(productsToReturn);
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
                var productId = fridgeProduct.IdProduct;
                var product = await _repository.Product.GetProductAsync(productId, false);
                var productForAddToFridgeDto = new ProductForAddToFridgeDto
                {
                    FridgeId = fridgeProduct.IdFridge,
                    Name = product.Name,
                    Quantity = product.DefaultQuantity
                };
                await AddProductInTheFridge(fridgeProduct.IdFridge, productForAddToFridgeDto);
            }
        }

       
        [HttpPut("/EditFridge/{idFridge}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> EditFridge(int idFridge,[FromBody] EditFridgeDto editFridgeDto)
        {
            var editFridge = await _repository.Fridge.GetFridgeAsync(idFridge, true);
            if (string.IsNullOrEmpty(editFridgeDto.Name) || string.IsNullOrEmpty(editFridgeDto.OwnerName))
            {
                _logger.LogInfo($"write old or new name/owner name");
                return NotFound(); 
            }

            _mapper.Map(editFridgeDto, editFridge);

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
            
            _repository.Fridge.DeleteFridge(fridge);
            await _repository.SaveAsync();
            return NoContent();
        }
        
        
        [HttpPost("CreateFridge")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateFridge([FromBody] FridgeForCreationDto fridge)
        {
            var newFridge = _mapper.Map<Fridge>(fridge);

            _repository.Fridge.CreateFridge(newFridge);
            await _repository.SaveAsync();
            
            var fridges = await _repository.Fridge.GetAllFridgesAsync(false);
            var fridgesDto = _mapper.Map<IEnumerable<FridgeDto>>(fridges);
            var idNewFridge = fridgesDto.OrderBy(x => x.Id).Last().Id;

            foreach (var item in fridge.ProductsId)
            {
                var product = await _repository.Product.GetProductAsync(item, false);

                if (product != null)
                {
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
            }
            
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
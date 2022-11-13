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

            List<FridgeWithModelDto> fridgeWithModelDtos = new List<FridgeWithModelDto>();
            var fridges = _repository.Fridge.GetAllFridges(trackChanges: false);
            foreach (var fridge in fridges)
            {
                var model = _repository.Model.GetModel(fridge.IdModel, false);
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
        
        
        [HttpPost("/AddProductInTheFridge/{fridgeId}", Name ="AddProductInTheFridge")]
        public IActionResult AddProductInTheFridge(int fridgeId,[FromBody] ProductForAddToFridgeDto productForAddToFridgeDto) {
            
            
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
                _repository.Save();
                var productsFromDb = _repository.Product.GetProducts(productForAddToFridgeDto.FridgeId, trackChanges: false);
                return Ok(productsFromDb);
                
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
                _repository.FridgeProduct.DeleteFridgeProduct(fridgeProduct);
                _repository.Save();
            }
            else
            {
                fridgeProduct.Quantity = fridgeProduct.Quantity - deleteProduct.Quantity;
                _repository.Save();
            }
            return NoContent();
        }

        
        [HttpPut("CallStoredProcedure")]
        public void CallStoredProcedure()
        {
            var badProducts = _repository.FridgeProduct.GetZeroFridgeProducts();

            foreach (var fridgeProduct in badProducts)
            {
                var product = _repository.Product.GetProduct(fridgeProduct.IdProduct, false);
                ProductForAddToFridgeDto productForAddToFridgeDto = new ProductForAddToFridgeDto
                {
                    FridgeId = fridgeProduct.IdFridge,
                    Name = product.Name,
                    Quantity = product.DefaultQuantity
                };

                AddProductInTheFridge(fridgeProduct.IdFridge, productForAddToFridgeDto);

            }

            // foreach (var fridgeProduct in badProducts)
            // {
            //     var rowForChange = _repository.FridgeProduct.GetFridgeProduct(fridgeProduct.IdProduct
            //         , fridgeProduct.IdFridge, true);
            //
            //     var defaultQuality = _repository.Product.GetProduct(fridgeProduct.IdProduct, false)
            //         .DefaultQuantity;
            //
            //     rowForChange.Quantity = defaultQuality;
            //     _repository.Save();
            // }
        }

        
        [HttpPut("/EditFridge/{idFridge}")]
        public IActionResult EditFridge(int idFridge,[FromBody] EditFridgeDto editFridgeDto)
        {
            var editFridge = _repository.Fridge.GetFridge(idFridge, true);
            // var fridge = _repository.Fridge.GetFridge(idFridge, true);
            if (string.IsNullOrEmpty(editFridgeDto.Name) || string.IsNullOrEmpty(editFridgeDto.OwnerName))
            {
                _logger.LogInfo($"write old or new name/owner name");
                return NotFound(); 
            }
            else
            {
                editFridge.Name = editFridgeDto.Name;
                editFridge.OwnerName = editFridgeDto.OwnerName;
                editFridge.IdModel = editFridgeDto.IdModel;
                _repository.Save();
            }

            foreach (var editedProduct in editFridgeDto.EditedProducts)
            {
                if (editedProduct.EditedQuantity == 0)
                {
                    var productOnFridge = _repository.FridgeProduct.GetFridgeProduct(editedProduct.IdProduct, idFridge, false);
                    _repository.FridgeProduct.DeleteFridgeProduct(productOnFridge);
                }
                else
                {
                    var productOnFridge = _repository.FridgeProduct.GetFridgeProduct(editedProduct.IdProduct, idFridge, true);
                    productOnFridge.Quantity = editedProduct.EditedQuantity;
                    // productOnFridge.Quantity = 0;
                }
                _repository.Save();
            }

            foreach (var idAddedProduct in editFridgeDto.AddedProducts)
            {
                var newProductForFridge = _repository.Product.GetProduct(idAddedProduct, false);
                var fridgeProduct = new FridgeProduct
                {
                    IdProduct = idAddedProduct,
                    IdFridge = idFridge,
                    Quantity = newProductForFridge.DefaultQuantity
                };
                _repository.FridgeProduct.AddProductIntoFridge(fridgeProduct);
                _repository.Save();
            }
            
            return NoContent();
        }
        
        
        [HttpDelete("DeleteFridge/{idFridge}")]
        public IActionResult DeleteFridge(int idFridge) {

            var fridge = _repository.Fridge.GetFridge(idFridge, trackChanges: false);
            
            if(fridge == null)
            {
                _logger.LogInfo($"error of input data");
                return NotFound(); 
            }
            
            var fridgeProducts = _repository.FridgeProduct.GetFridgeProducts(false, idFridge: idFridge);

            foreach (var fridgeProduct in fridgeProducts)
            {
                _repository.FridgeProduct.DeleteFridgeProduct(fridgeProduct);
            }
            _repository.Fridge.DeleteFridge(fridge);
            _repository.Save();
            return NoContent();
        }
        
        
        [HttpPost("CreateFridge")]
        public IActionResult CreateFridge([FromBody] FridgeForCreationDto fridge)
        {
            if (fridge == null)
            {
                _logger.LogError("FridgeForCreationDto object sent from client is null.");
                return BadRequest("FridgeForCreationDto object is null");
            }

            if (!ModelState.IsValid) {
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
            _repository.Save();
            
            var idNewFridge = _repository.Fridge.GetAllFridges(false).OrderBy(x => x.Id).Last().Id;

            foreach (var item in fridge.ProductsId)
            {
                var product = _repository.Product.GetProduct(item, false);

                var fridgeProduct = new FridgeProduct
                {
                    IdProduct = item,
                    IdFridge = idNewFridge,
                    Quantity = product.DefaultQuantity
                    
                    //for test Stored Procedure
                    //Quantity = 0
                };
                
                _repository.FridgeProduct.AddProductIntoFridge(fridgeProduct);
            }
            
            _repository.Save();

            return NoContent();
        }
    }
}
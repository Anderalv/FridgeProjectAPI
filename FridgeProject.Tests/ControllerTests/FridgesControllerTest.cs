using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Fridge;
using Application.DTOs.Product;
using AutoMapper;
using Contracts;
using Domain.Entities;
using FridgeProject.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using Xunit;

namespace FridgeProject.Tests.ControllerTests
{
    public class FridgesControllerTest
    {
        private static IMapper _mapper;

        public FridgesControllerTest()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new SourceMappingProfile()); });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }
        

        [Fact]
        public void GetFridges_FridgesInDataBase_GetAllThisFridges()
        {
            // Arrange
            Mock<ILoggerManager> mockLogger = new Mock<ILoggerManager>();
            Mock<IRepositoryManager> mockRepository = new Mock<IRepositoryManager>();
            mockRepository.Setup(x=> x.Fridge.GetAllFridgesAsync(false)).ReturnsAsync(GetTestFridges());
            mockRepository.Setup(x=> x.Model.GetModelAsync(1, false)).ReturnsAsync(GetTestModels().First(x => x.Id == 1));
            mockRepository.Setup(x=> x.Model.GetModelAsync(2, false)).ReturnsAsync(GetTestModels().First(x => x.Id == 2));
            
            var controller = new FridgesController(mockRepository.Object, mockLogger.Object, _mapper);
 
            // Act
            var result = controller.GetFridges();
 
            // Assert
            var viewResult = Assert.IsType<Task<IActionResult>>(result);
            var viewResult2 = Assert.IsType<OkObjectResult>(viewResult.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<FridgeWithModelDto>>(viewResult2.Value);
            Assert.Equal(GetTestFridges().Count, model.Count());
        }
        
        
        [Fact]
        public void GetProductsFromTheFridge_ProductsFromFridgeInDataBase_GetAllProductsFromThisFridge()
        {
            // Arrange
            Mock<ILoggerManager> mockLogger = new Mock<ILoggerManager>();
            Mock<IRepositoryManager> mockRepository = new Mock<IRepositoryManager>();
            mockRepository.Setup(x => x.Fridge.GetFridgeAsync(1, false))
                .ReturnsAsync(GetTestFridges().First(x => x.Id == 1));
            mockRepository.Setup(x => x.Product.GetProductsAsync(1, false)).ReturnsAsync(GetTestProducts());
            
            var controller = new FridgesController(mockRepository.Object, mockLogger.Object, _mapper);
 
            // Act
            var result = controller.GetProductsFromTheFridge(1);
 
            // Assert
            var viewResult = Assert.IsType<Task<IActionResult>>(result);
            var viewResult2 = Assert.IsType<OkObjectResult>(viewResult.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductInFridgeDtoToReturn>>(viewResult2.Value);
            Assert.Equal(GetTestProducts().Count, model.Count());
        }
        
        
        [Fact]
        public void AddProductInTheFridge_FridgeWithoutProducts_FridgeWithOneProduct()
        {
            // Arrange
            Mock<ILoggerManager> mockLogger = new Mock<ILoggerManager>();
            Mock<IRepositoryManager> mockRepository = new Mock<IRepositoryManager>();
            
            ProductForAddToFridgeDto productForAddToFridgeDto = new ProductForAddToFridgeDto
            {
                FridgeId = 1,
                Name = "Product1",
                Quantity = 1
            };
            
            mockRepository.Setup(x => x.Product.GetProductByNameAsync(productForAddToFridgeDto.Name, false))
                .ReturnsAsync(_product);
            
            mockRepository.Setup(x => x.FridgeProduct.GetFridgeProductAsync(4, productForAddToFridgeDto.FridgeId, true))
                .ReturnsAsync(_fridgeProduct);

            mockRepository.Setup(x => x.Product.GetProductsAsync(productForAddToFridgeDto.FridgeId, false))
                .ReturnsAsync(NewProductInFridge());

            
            var controller = new FridgesController(mockRepository.Object, mockLogger.Object, _mapper);
 
            // Act
            var result = controller.AddProductInTheFridge(1, productForAddToFridgeDto);
 
            // Assert
            var viewResult = Assert.IsType<Task<IActionResult>>(result);
            var viewResult2 = Assert.IsType<OkObjectResult>(viewResult.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductInFridgeDtoToReturn>>(viewResult2.Value);
            Assert.Equal(NewProductInFridge().First().Quantity, model.First().Quantity);
            Assert.Equal(NewProductInFridge().First().Name, model.First().Name);
        }
        
        
        [Fact]
        public void AddProductInTheFridge_FridgeWithOneProduct_FridgeWithThinProductWithNewQuantity()
        {
            // Arrange
            Mock<ILoggerManager> mockLogger = new Mock<ILoggerManager>();
            Mock<IRepositoryManager> mockRepository = new Mock<IRepositoryManager>();
            
            ProductForAddToFridgeDto productForAddToFridgeDto = new ProductForAddToFridgeDto
            {
                FridgeId = 1,
                Name = "Product1",
                Quantity = 1
            };

            mockRepository.Setup(x => x.Product.GetProductByNameAsync(productForAddToFridgeDto.Name, false))
                .ReturnsAsync(_product);            
            
            mockRepository.Setup(x => x.FridgeProduct.GetFridgeProductAsync(4, productForAddToFridgeDto.FridgeId, true))
                .ReturnsAsync(_fridgeProduct);
                      
            mockRepository.Setup(x => x.FridgeProduct.GetFridgeProductAsync(4, productForAddToFridgeDto.FridgeId, false))
                .ReturnsAsync(_fridgeProduct);                      
            
            mockRepository.Setup(x => x.FridgeProduct.GetFridgeProductAsync(4, productForAddToFridgeDto.FridgeId, false))
                .ReturnsAsync(_fridgeProduct);
          
            mockRepository.Setup(x => x.Product.GetProductsAsync(productForAddToFridgeDto.FridgeId, false))
                .ReturnsAsync(ProductWithNewCount());
            
            var controller = new FridgesController(mockRepository.Object, mockLogger.Object, _mapper);
 
            // Act
            var result = controller.AddProductInTheFridge(1, productForAddToFridgeDto);
 
            // Assert
            var viewResult = Assert.IsType<Task<IActionResult>>(result);
            var viewResult2 = Assert.IsType<OkObjectResult>(viewResult.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductInFridgeDtoToReturn>>(viewResult2.Value);
            Assert.Equal(ProductWithNewCount().First().Quantity, model.First().Quantity);
            Assert.Equal(ProductWithNewCount().First().Name, model.First().Name);
        }
        
        
        private Product _product = new Product {
            Id = 4,
            Name = "Product1",
            DefaultQuantity = 4
        };
        
        private List<Fridge> GetTestFridges()
        {
            var fridges = new List<Fridge>
            {
                new Fridge
                {
                    Id = 1,
                    IdModel = 1,
                    Name = "Fridge1",
                    OwnerName = "Ivan"
                }, 
                new Fridge
                {
                    Id = 2,
                    IdModel = 1,
                    Name = "Fridge2",
                    OwnerName = "Andrey"
                },
                new Fridge
                {
                    Id = 3,
                    IdModel = 2,
                    Name = "Fridge3",
                    OwnerName = "Dima"
                }
            };
            return fridges;
        }

        private List<Model> GetTestModels()
        {
            var models = new List<Model>
            {
                new Model
                {
                    Id = 1,
                    Name = "Model1",
                    Year = 2001
                },
                new Model
                {
                    Id = 2,
                    Name = "Model2",
                    Year = 2002
                }
            };
            return models;
        }

        private List<ProductInFridgeDto> GetTestProducts()
        {
            var products = new List<ProductInFridgeDto>
            {
                new ProductInFridgeDto
                {
                    Id = 1,
                    Name = "Product1",
                    Quantity = 1,
                },

                new ProductInFridgeDto
                {
                    Id = 2,
                    Name = "Product2",
                    Quantity = 2,
                },

                new ProductInFridgeDto
                {
                    Id = 3,
                    Name = "Product3",
                    Quantity = 3,
                }
            };
            return products;
        }

        private FridgeProduct _fridgeProduct = new FridgeProduct
        {
            Id = 1,
            IdProduct = 4,
            IdFridge = 1,
            Quantity = 2
        };

        private IEnumerable<ProductInFridgeDto> ProductWithNewCount()
        {
            var product = new List<ProductInFridgeDto>
            {
                new ProductInFridgeDto
                {
                    Name = "Product1",
                    Quantity = 3
                }
            };
            return product;
        }
        
        private IEnumerable<ProductInFridgeDto> NewProductInFridge()
        {
            var product = new List<ProductInFridgeDto>
            {
                new ProductInFridgeDto
                {
                    Name = "Product1",
                    Quantity = 1
                }
            };
            return product;
        }
        
    }
}
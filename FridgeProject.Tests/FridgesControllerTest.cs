using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using FridgeProject.Controllers;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NSubstitute;
using NSubstitute.Core;
using Repository;
using Xunit;

namespace FridgeProject.Tests
{
    public class FridgesControllerTest //: IClassFixture<ServerFixture>, IDisposable
    {
        [Fact]
        public void IndexReturnsAViewResultWithAListOfFridges()
        {
            // Arrange
            var repositoryManager = Substitute.For<IRepositoryManager>();
            repositoryManager.Fridge.GetAllFridgesAsync(false).Returns(GetTestFridges());
            repositoryManager.Model.GetModelAsync(1, false).Returns(GetTestModels().First(x => x.Id == 1));
            repositoryManager.Model.GetModelAsync(2, false).Returns(GetTestModels().First(x => x.Id == 2));
            var controller = new FridgesController(repositoryManager, Substitute.For<ILoggerManager>());
 
            // Act
            var result = controller.GetFridges();
 
            // Assert
            var viewResult = Assert.IsType<Task<IActionResult>>(result);
            var viewResult2 = Assert.IsType<OkObjectResult>(viewResult.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<FridgeWithModelDto>>(viewResult2.Value);
            Assert.Equal(GetTestFridges().Count, model.Count());
        }
        
        [Fact]
        public void IndexReturnsAViewResultWithAGetProductsFromTheFridge()
        {
            // Arrange
            var repositoryManager = Substitute.For<IRepositoryManager>();
            repositoryManager.Fridge.GetFridgeAsync(1 ,false).Returns(GetTestFridges().First(x => x.Id == 1));

            repositoryManager.Product.GetProductsAsync(1, false).Returns(GetTestProducts());
            var controller = new FridgesController(repositoryManager, Substitute.For<ILoggerManager>());
 
            // Act
            var result = controller.GetProductsFromTheFridge(1);
 
            // Assert
            var viewResult = Assert.IsType<Task<IActionResult>>(result);
            var viewResult2 = Assert.IsType<OkObjectResult>(viewResult.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductInFridgeDto>>(viewResult2.Value);
            Assert.Equal(GetTestProducts().Count, model.Count());
        }
        
        [Fact]
        public void IndexReturnsAViewResultWithAAddProductInTheFridge()
        {
            // Arrange
            var repositoryManager = Substitute.For<IRepositoryManager>();
            ProductForAddToFridgeDto productForAddToFridgeDto = new ProductForAddToFridgeDto
            {
                FridgeId = 1,
                Name = "Product1",
                Quantity = 1
            };

            repositoryManager.Product.GetProductByNameAsync(productForAddToFridgeDto.Name, false)
                .Returns(_product);
            repositoryManager.FridgeProduct.GetFridgeProductAsync(4, productForAddToFridgeDto.FridgeId, true)
                .Returns(_fridgeProduct);

            var controller = new FridgesController(repositoryManager, Substitute.For<ILoggerManager>());
 
            // Act
            var result = controller.AddProductInTheFridge(1, productForAddToFridgeDto);
 
            // Assert
            var viewResult = Assert.IsType<Task<IActionResult>>(result);
            var viewResult2 = Assert.IsType<OkObjectResult>(viewResult.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductInFridgeDto>>(viewResult2.Value);
        }
        
        [Fact]
        public void IndexReturnsAViewResultWithAAddProductInTheFridge2()
        {
            // Arrange
            var repositoryManager = Substitute.For<IRepositoryManager>();
            ProductForAddToFridgeDto productForAddToFridgeDto = new ProductForAddToFridgeDto
            {
                FridgeId = 1,
                Name = "Product1",
                Quantity = 1
            };

            repositoryManager.Product.GetProductByNameAsync(productForAddToFridgeDto.Name, false)
                .Returns(_product);
            repositoryManager.FridgeProduct.GetFridgeProductAsync(4, productForAddToFridgeDto.FridgeId, true)
                .Returns(_fridgeProduct);
            repositoryManager.FridgeProduct.GetFridgeProductAsync(4, productForAddToFridgeDto.FridgeId, false)
                .Returns(_fridgeProduct);
                

            var controller = new FridgesController(repositoryManager, Substitute.For<ILoggerManager>());
 
            // Act
            var result = controller.AddProductInTheFridge(1, productForAddToFridgeDto);
 
            // Assert
            var viewResult = Assert.IsType<Task<IActionResult>>(result);
            var viewResult2 = Assert.IsType<OkObjectResult>(viewResult.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductInFridgeDto>>(viewResult2.Value);
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
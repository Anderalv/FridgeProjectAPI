using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class ProductControllerTest
    {
        private static IMapper _mapper;

        public ProductControllerTest()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new SourceMappingProfile()); });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }
        
        
        [Fact]
        public void GetProduct_ProductOnDataBase_GetProductByIdFromDataBase()
        {
            // Arrange
            Mock<ILoggerManager> mockLogger = new Mock<ILoggerManager>();
            Mock<IRepositoryManager> mockRepository = new Mock<IRepositoryManager>();
            mockRepository.Setup(x => x.Product.GetProductAsync(1, false))
                .ReturnsAsync(GetTestProducts().First(x=>x.Id == 1));
            
            var controller = new ProductController(mockRepository.Object, mockLogger.Object, _mapper);
 
            // Act
            var result = controller.GetProduct(1);
 
            // Assert
            var viewResult = Assert.IsType<Task<IActionResult>>(result);
            var viewResult2 = Assert.IsType<OkObjectResult>(viewResult.Result);
            var model = Assert.IsAssignableFrom<ProductDto>(viewResult2.Value);
            Assert.Equal(1, model.Id);
        }
        
        
        [Fact]
        public void CreateProduct_DataBaseWithoutThisProduct_NewProductInDataBase()
        {
            // Arrange
            Mock<ILoggerManager> mockLogger = new Mock<ILoggerManager>();
            Mock<IRepositoryManager> mockRepository = new Mock<IRepositoryManager>();

            var controller = new ProductController(mockRepository.Object, mockLogger.Object, _mapper);
            ProductForCreateDto productForCreateDto = new ProductForCreateDto
            {
                Name = "ProductForCreation",
                Quantity = 10
            };
            
            mockRepository.Setup(x => x.Product.CreateProduct(new Product()));

            // Act
            var result = controller.CreateProduct(productForCreateDto);
            
            // Assert
            var viewResult = Assert.IsType<Task<IActionResult>>(result);
            var viewResult2 = Assert.IsType<CreatedAtRouteResult>(viewResult.Result);
            var model = Assert.IsAssignableFrom<ProductDto>(viewResult2.Value);
            Assert.Equal(productForCreateDto.Name, model.Name);
            Assert.Equal(productForCreateDto.Quantity, model.DefaultQuantity);
        }
        
        
        [Fact]
        public void GetAllProducts_ProductsOnDataBase_GetAllProductsFromDataBase()
        {
            // Arrange
            Mock<ILoggerManager> mockLogger = new Mock<ILoggerManager>();
            Mock<IRepositoryManager> mockRepository = new Mock<IRepositoryManager>();
            
            mockRepository.Setup(x => x.Product.GetAllProductsAsync(false)).ReturnsAsync(GetTestProducts());
            
            var controller = new ProductController(mockRepository.Object, mockLogger.Object, _mapper);

            // Act
            var result = controller.GetAllProducts();
            
            // Assert
            var viewResult = Assert.IsType<Task<IActionResult>>(result);
            var viewResult2 = Assert.IsType<OkObjectResult>(viewResult.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(viewResult2.Value);
            Assert.Equal(GetTestProducts().Count, model.Count());
        }
        
        
        [Fact]
        public void UpdateProduct_ProductWithOldNameAndQuantity_ThisProductWithNewNameAndQuantity()
        {
            // Arrange
            Mock<ILoggerManager> mockLogger = new Mock<ILoggerManager>();
            Mock<IRepositoryManager> mockRepository = new Mock<IRepositoryManager>();
            
            mockRepository.Setup(x => x.Product.GetProductAsync(1, true)).ReturnsAsync(GetTestProducts().First(x => x.Id == 1));

            var controller = new ProductController(mockRepository.Object, mockLogger.Object, _mapper);
            ProductForUpdateDto productForUpdateDto = new ProductForUpdateDto
            {
                Name = "ProductForUpdateDto",
                Quantity = 10
            };
            
            // Act
            var result = controller.UpdateProduct(1, productForUpdateDto);
            
            // Assert
            var viewResult = Assert.IsType<Task<IActionResult>>(result);
            var viewResult2 = Assert.IsType<NoContentResult>(viewResult.Result);
            Assert.Equal(204, viewResult2.StatusCode);
        }
        

        private Product _product = new Product
        {
            Id = 4,
            Name = "product4",
            DefaultQuantity = 4
        };

        private List<Product> GetTestProducts()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Product1",
                    DefaultQuantity = 1,
                },

                new Product
                {
                    Id = 2,
                    Name = "Product2",
                    DefaultQuantity = 2,
                },

                new Product
                {
                    Id = 3,
                    Name = "Product3",
                    DefaultQuantity = 3,
                }
            };
            return products;
        }
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
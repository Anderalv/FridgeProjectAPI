using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using FridgeProject.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace FridgeProject.Tests
{
    public class ProductControllerTest
    {
        [Fact]
        public void IndexReturnsAViewResultWithProduct()
        {
            // Arrange
            var repositoryManager = Substitute.For<IRepositoryManager>();
            repositoryManager.Product.GetProductAsync(1, false).Returns(GetTestProducts().First(x=>x.Id == 1));
            var controller = new ProductController(repositoryManager, Substitute.For<ILoggerManager>(), Substitute.For<IMapper>());
 
            // Act
            var result = controller.GetProduct(1);
 
            // Assert
            var viewResult = Assert.IsType<Task<IActionResult>>(result);
            var viewResult2 = Assert.IsType<OkObjectResult>(viewResult.Result);
            var model = Assert.IsAssignableFrom<ProductDto>(viewResult2.Value);
            Assert.Equal(1, model.Id);
        }
        
        [Fact]
        public void IndexReturnsAViewResultWithACreateProduct()
        {
            // Arrange
            var repositoryManager = Substitute.For<IRepositoryManager>();
            var controller = new ProductController(repositoryManager, Substitute.For<ILoggerManager>(), Substitute.For<IMapper>());
            ProductForCreateDto productForCreateDto = new ProductForCreateDto
            {
                Name = "ProductForCreation",
                Quantity = 10
            };


            // Act
            var result = controller.CreateProduct(productForCreateDto);
            
            // Assert
            var viewResult = Assert.IsType<Task<IActionResult>>(result);
            var viewResult2 = Assert.IsType<CreatedAtRouteResult>(viewResult.Result);
        }
        
        [Fact]
        public void IndexReturnsAViewResultWithAListOfProducts()
        {
            // Arrange
            var repositoryManager = Substitute.For<IRepositoryManager>();
            repositoryManager.Product.GetAllProductsAsync(false).Returns(GetTestProducts());
            var controller = new ProductController(repositoryManager, Substitute.For<ILoggerManager>(), Substitute.For<IMapper>());

            // Act
            var result = controller.GetAllProducts();
            
            // Assert
            var viewResult = Assert.IsType<Task<IActionResult>>(result);
            var viewResult2 = Assert.IsType<OkObjectResult>(viewResult.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult2.Value);
            Assert.Equal(GetTestProducts().Count, model.Count());
        }
        
        [Fact]
        public void IndexReturnsAViewResultWithAUpdateProduct()
        {
            // Arrange
            var repositoryManager = Substitute.For<IRepositoryManager>();
            repositoryManager.Product.GetProductAsync(1, true).Returns(GetTestProducts().First(x => x.Id == 1));
            var controller = new ProductController(repositoryManager, Substitute.For<ILoggerManager>(), Substitute.For<IMapper>());
            ProductForUpdateDto productForUpdateDto = new ProductForUpdateDto
            {
                Name = "ProductForUpdateDto",
                Quantity = 10
            };
            
            // Act
            var result = controller.UpdateProduct(1, productForUpdateDto);
            
            // Assert
            var viewResult = Assert.IsType<Task<IActionResult>>(result);
            var viewResult2 = Assert.IsType<OkObjectResult>(viewResult.Result);
            var model = Assert.IsAssignableFrom<Product>(viewResult2.Value);
            Assert.Equal("ProductForUpdateDto", model.Name);
            Assert.Equal(10, model.DefaultQuantity);
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
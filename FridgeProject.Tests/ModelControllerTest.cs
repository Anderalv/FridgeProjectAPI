using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Contracts;
using Entities.Models;
using FridgeProject.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace FridgeProject.Tests
{
    public class ModelControllerTest
    {
        [Fact]
        public void IndexReturnsAViewResultWithAListOfProducts()
        {
            // Arrange
            var repositoryManager = Substitute.For<IRepositoryManager>();
            repositoryManager.Model.GetAllFridgeModels(false).Returns(GetTestModels());
            var controller = new ModelController(repositoryManager);

            // Act
            var result = controller.GetAllModels();
            
            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Model>>(viewResult.Value);
            Assert.Equal(GetTestModels().Count, model.Count());
        }
        
        private List<Model> GetTestModels()
        {
            var models = new List<Model>
            {
                new Model
                {
                    Id = 1,
                    Name = "Model1"
                },

                new Model
                {
                    Id = 2,
                    Name = "Model2"
                },

                new Model
                {
                    Id = 3,
                    Name = "Model3"
                }
            };
            return models;
        }
    }
}
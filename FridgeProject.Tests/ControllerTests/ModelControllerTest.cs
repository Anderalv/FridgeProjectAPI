using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Model;
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
    public class ModelControllerTest
    {
        private static IMapper _mapper;

        public ModelControllerTest()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new SourceMappingProfile()); });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }


        [Fact]
        public void GetAllModels_ModelsInDataBase_GetAllModelsFromDataBase()
        {
            // Arrange
            Mock<IRepositoryManager> mockRepository = new Mock<IRepositoryManager>();
            mockRepository.Setup(x => x.Model.GetAllFridgeModelsAsync(false)).ReturnsAsync(GetTestModels());
            
            var controller = new ModelController(mockRepository.Object, _mapper);

            // Act
            var result = controller.GetAllModels();

            //Assert
            var viewResult = Assert.IsType<Task<IActionResult>>(result);
            var viewResult2 = Assert.IsType<OkObjectResult>(viewResult.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<ModelDto>>(viewResult2.Value);
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
using System.Linq;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FridgeProject.Controllers
{
    [Route("api/Model")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        
        public ModelController(IRepositoryManager repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAllModels()
        {
            var models = _repository.Model.GetAllFridgeModels(false).ToList();
            return Ok(models);
        }
    }
}
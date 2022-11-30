using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FridgeProject.Controllers
{
    [Route("api/Model")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        
        public ModelController(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllModels()
        {
            var models = await _repository.Model.GetAllFridgeModelsAsync(false);
            var modelsDto = _mapper.Map<IEnumerable<ModelDto>>(models);
            return Ok(modelsDto);
        }
    }
}
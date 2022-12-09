using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Model;
using AutoMapper;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
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
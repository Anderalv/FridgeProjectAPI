using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FridgeProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase {
        private readonly IRepositoryManager _repository;
        public WeatherForecastController(IRepositoryManager repository) {
            _repository = repository; }
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get() {
            // _repository.Fridge.AnyMethodFromFridgeRepository(); 
            // _repository.Model.AnyMethodFromModelRepository();
            return new string[] { "value1", "value2" }; }
    }
}
using System;
using System.Threading.Tasks;
using Application.Behaviours.ActionFilters;
using Application.DTOs.Account;
using AutoMapper;
using Contracts;
using Domain.Entities;
using Entities.DataTransferObjects;
using Infrastructure.Identity.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase 
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationManager _authManager;
        private readonly RepositoryContext _repository;

        public AuthenticationController(ILoggerManager logger, IMapper mapper, UserManager<User> userManager,
            IAuthenticationManager authManager, RepositoryContext repository)
        {
            _logger = logger; 
            _mapper = mapper;
            _userManager = userManager;
            _authManager = authManager;
            _repository = repository;
        }
        
        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password); 
            
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors) {
                    ModelState.TryAddModelError(error.Code, error.Description); 
                }
                return BadRequest(ModelState); 
            }
            await _userManager.AddToRolesAsync(user, userForRegistration.Roles); 
            return StatusCode(201);
        }
        
        [HttpPost]
        [Route("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user) 
        {
            if (!await _authManager.ValidateUser(user)) 
            {
                _logger.LogWarn($"{nameof(Authenticate)}: Authentication failed. Wrong user name or password.");
                return Unauthorized();
            }
            var refreshToken = _authManager.CreateRefreshToken();
            var authenticatedUser = await _userManager.FindByNameAsync(user.UserName);
            authenticatedUser.RefreshToken = refreshToken;
            authenticatedUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _repository.SaveChangesAsync();
            var token = await _authManager.CreateToken(authenticatedUser);
            
            return Ok(new AuthenticatedResponseDto()
            {
                Token = token,
                RefreshToken = refreshToken
            }); 
        }
    }
}
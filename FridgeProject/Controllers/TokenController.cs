using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FridgeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly RepositoryContext _repository;
        private readonly ITokenService _tokenService;
        private readonly IAuthenticationManager _authManager;
        
        public TokenController(ITokenService tokenService, RepositoryContext repository, IAuthenticationManager authManager)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _authManager = authManager;
            
        }
        
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid client request");
            
            var accessToken = tokenApiModel.AccessToken;
            var refreshToken = tokenApiModel.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
           
            var user = _repository.Users.SingleOrDefault(u => u.UserName == username);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");

            var newAccessToken = await _authManager.CreateToken(user);
            var newRefreshToken = _authManager.CreateRefreshToken();
            
            user.RefreshToken = newRefreshToken;
            await _repository.SaveChangesAsync();
            
            return Ok(new AuthenticatedResponseDto()
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
        
        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;
            var user = _repository.Users.SingleOrDefault(u => u.UserName == username);
            if (user == null) return BadRequest();
            user.RefreshToken = null;
            _repository.SaveChanges();
            return NoContent();
        }
    }
}
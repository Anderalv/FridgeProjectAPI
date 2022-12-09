using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Domain.Entities;
using Entities.DataTransferObjects;
using Infrastructure.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Services
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<User> _userManager; 
        private readonly IConfiguration _configuration;
        private User _user;
        private readonly ITokenService _tokenService;
        
        public AuthenticationManager(UserManager<User> userManager, IConfiguration configuration, ITokenService tokenService)
        {
            _userManager = userManager; 
            _configuration = configuration;
            _tokenService = tokenService;
        }
        
        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth) {
            _user = await _userManager.FindByNameAsync(userForAuth.UserName); 
            return (_user != null && await _userManager.CheckPasswordAsync(_user,
                userForAuth.Password)); 
        }  
        
        public async Task<string> CreateToken(User user)
        {
            _user = user;
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions); 
        }

        public string CreateRefreshToken()
        {
            return _tokenService.GenerateRefreshToken();
        }

        private SigningCredentials GetSigningCredentials() {
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        
        private async Task<List<Claim>> GetClaims() 
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, _user.UserName) 
            };
            
            var roles = await _userManager.GetRolesAsync(_user); 
            
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); 
            }
            return claims; 
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken (
                issuer: jwtSettings.GetSection("validIssuer").Value, 
                audience: jwtSettings.GetSection("validAudience").Value, 
                claims: claims,
                expires:
                DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expires").Value)), 
                signingCredentials: signingCredentials
            );
            
            return tokenOptions; 
        }
    }
}
using System.Threading.Tasks;
using Domain.Entities;
using Entities.DataTransferObjects;

namespace Infrastructure.Identity.Interfaces
{
    public interface IAuthenticationManager {
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
        Task<string> CreateToken(User user);
        public string CreateRefreshToken();
    }
}
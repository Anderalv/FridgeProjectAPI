using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.Models;

namespace Contracts
{
    public interface IAuthenticationManager {
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
        Task<string> CreateToken(User user);
        public string CreateRefreshToken();
    }
}
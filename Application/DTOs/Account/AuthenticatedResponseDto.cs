namespace Entities.DataTransferObjects
{
    public class AuthenticatedResponseDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
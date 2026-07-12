
namespace SelfManagement.Application.DTO.Common
{
    public class JwtConfigurationResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; } = null!;

        public DateTime ExpiredAt { get; set; }
    }
}

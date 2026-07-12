
namespace SelfManagement.Application.DTO.Auth
{
    public class LoginUserResponse
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ProfileUrl { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }

    }
}

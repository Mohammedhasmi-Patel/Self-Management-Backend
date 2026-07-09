
using System.ComponentModel.DataAnnotations;

namespace SelfManagement.Application.DTO.Auth
{
    public class VerifyOtpRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Range(100000, 999999, ErrorMessage = "OTP must be a 6-digit number.")]
        public int Otp { get; set; }
    }
}

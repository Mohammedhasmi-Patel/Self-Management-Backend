
using System.ComponentModel.DataAnnotations;

namespace SelfManagement.Application.DTO.Auth
{
    public class LoginUserRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email must be in valid format.")]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 8,ErrorMessage = "Password must be between 8 and 30 characters.")]
        
        public string Password { get; set; } = null!;

    }
}

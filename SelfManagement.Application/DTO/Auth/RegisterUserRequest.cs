
using System.ComponentModel.DataAnnotations;

namespace SelfManagement.Application.DTO.Auth
{
    public class RegisterUserRequest
    {

        [Required(ErrorMessage = "FirstName is required.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = null!;
    }
}

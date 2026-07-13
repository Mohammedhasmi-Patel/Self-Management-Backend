

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SelfManagement.Application.DTO.Company
{
    public class CreateCompanyRequest
    {
        [Required(ErrorMessage = "Company name is required.")]
        [MinLength(3, ErrorMessage = "Company name must be at least 3 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        [MinLength(3, ErrorMessage = "Description must be at least 3 characters.")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Company logo is required.")]
        public IFormFile Logo { get; set; } = null!;

        [Url(ErrorMessage = "Please enter a valid website URL.")]
        public string? WebsiteUrl { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email must be in valid format.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Address is required.")]
        [MinLength(5, ErrorMessage = "Address must be at least 5 characters.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Country is required.")]
        public Guid CountryId { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public Guid StateId { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public Guid CityId { get; set; }
    }
}

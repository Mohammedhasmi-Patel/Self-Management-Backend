

using Microsoft.AspNetCore.Http;
using SelfManagement.Application.Validation;
using System.ComponentModel.DataAnnotations;

namespace SelfManagement.Application.DTO.Company
{
    public class CompanyUpdateRequest
    {
        [NotEmptyGuid]
        public Guid Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(3)]
        public string Description { get; set; } = null!;

        public IFormFile? Logo { get; set; } // Optional

        [Url]
        public string? WebsiteUrl { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(5)]
        public string Address { get; set; } = null!;

        [NotEmptyGuid]
        public Guid CountryId { get; set; }

        [NotEmptyGuid]
        public Guid StateId { get; set; }

        [NotEmptyGuid]
        public Guid CityId { get; set; }
    }
}

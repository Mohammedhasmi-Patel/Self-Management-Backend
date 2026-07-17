
namespace SelfManagement.Application.DTO.Company
{
    public class CompanyListResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public Guid CountryId { get; set; } 
        public Guid StateId { get; set; } 
        public Guid CityId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

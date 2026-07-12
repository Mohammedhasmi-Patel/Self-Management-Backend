
namespace SelfManagement.Domain.Entities
{
    public class Company : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? WebsiteUrl { get; set; }

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
        public Guid CountryId { get; set; }
        public Guid StateId { get; set; }
        public Guid CityId { get; set; }

        public int EmployeeCount { get; set; }
        public bool IsActive { get; set; } = true;

        public ApplicationUser User { get; set; } = null!;
        public Country Country { get; set; } = null!;

        public State State { get; set; } = null!;

        public City City { get; set; } = null!;


    }
}

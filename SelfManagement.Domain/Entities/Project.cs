

namespace SelfManagement.Domain.Entities
{
    public class Project : BaseEntity
    {
        public Guid Id { get; set; }

        public string Slug { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string UserId { get; set; } = null!;

        public Guid CompanyId { get; set; }
        public string? RepositoryUrl { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Notes { get; set; }

        // Navigation Properties
        public ApplicationUser User { get; set; } = null!;

        public Company Company { get; set; } = null!;

    }
}

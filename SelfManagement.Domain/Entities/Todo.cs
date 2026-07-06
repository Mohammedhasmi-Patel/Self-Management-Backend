
using SelfManagement.Domain.Enum;

namespace SelfManagement.Domain.Entities
{
    public class Todo : BaseEntity
    {
        public Guid Id {  get; set; }
        public string Title { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string? Description { get; set; }

        public Guid UserId { get; set; }

        public Guid CompanyId { get; set; }

        public Guid ProjectId { get; set; }

        public Guid CategoryId { get; set; }

        public string Status { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        public DateTime? CompletedAt { get; set; }

        // Navigation Properties
        public ApplicationUser User { get; set; } = null!;

        public Company Company { get; set; } = null!;

        public Project Project { get; set; } =  null!;

        public Category Category { get; set; } = null!;
    }
}

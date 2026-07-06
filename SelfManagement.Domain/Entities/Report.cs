namespace SelfManagement.Domain.Entities
{
    public class Report : BaseEntity
    {
        public string Slug { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public Guid? CompanyId { get; set; }

        public Guid? ProjectId { get; set; }

        public Guid? CategoryId { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateOnly? ReportDate { get; set; }

        public string? Summary { get; set; }

        public string? Challenges { get; set; }

        public string? Learnings { get; set; }

        // Navigation Properties
        public ApplicationUser User { get; set; } = null!;

        public Company Company { get; set; } = null!;


        public Project Project { get; set; } = null!;

        public Category Category { get; set; } = null!;
    }
}

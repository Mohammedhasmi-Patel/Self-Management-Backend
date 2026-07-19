using System.ComponentModel.DataAnnotations;

namespace SelfManagement.Application.DTO.Project
{
    public class CreateProjectRequest
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [Url]
        public string? RepositoryUrl { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? Status { get; set; }
        public string? Notes { get; set; }
    }
}

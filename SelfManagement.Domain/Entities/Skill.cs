
namespace SelfManagement.Domain.Entities
{
    public class Skill : BaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public Guid CategoryId { get; set; }

        public string? Description { get; set; }

        public Category SkillCategory { get; set; } = null!;

    }
}

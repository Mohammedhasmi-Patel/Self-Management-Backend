
namespace SelfManagement.Domain.Entities
{
    public class ProjectSkill : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SkillId { get; set; }

        // Navigation Properties
        public Project Project { get; set; } = null!;

        public Category Category { get; set; } = null!;

        public Skill Skill { get; set; } = null!;

    }
}

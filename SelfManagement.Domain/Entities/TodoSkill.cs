

namespace SelfManagement.Domain.Entities
{
    public class TodoSkill : BaseEntity
    {
        public Guid TodoId { get; set; }

        public Guid SkillId { get; set; }

        // Navigation Properties
        public Todo Todo { get; set; } = null!;

        public Skill Skill { get; set; } = null!;
    }
}

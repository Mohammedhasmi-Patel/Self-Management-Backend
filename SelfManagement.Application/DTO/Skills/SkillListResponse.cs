using System;

namespace SelfManagement.Application.DTO.Skills
{
    public class SkillListResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

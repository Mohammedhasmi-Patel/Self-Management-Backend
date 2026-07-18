using System.ComponentModel.DataAnnotations;

namespace SelfManagement.Application.DTO.Skills
{
    public class CreateSkillRequest
    {
        [Required(ErrorMessage = "Skill name is required.")]
        [MinLength(2, ErrorMessage = "Skill name must be at least 2 characters.")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public Guid CategoryId { get; set; }
    }
}

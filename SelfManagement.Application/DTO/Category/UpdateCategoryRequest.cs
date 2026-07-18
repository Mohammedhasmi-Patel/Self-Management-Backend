using System.ComponentModel.DataAnnotations;

namespace SelfManagement.Application.DTO.Categories
{
    public class UpdateCategoryRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [MinLength(2, ErrorMessage = "Category name must be at least 2 characters.")]
        public string Name { get; set; } = null!;
    }
}

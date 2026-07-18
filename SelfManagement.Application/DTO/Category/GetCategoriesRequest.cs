using System.ComponentModel.DataAnnotations;

namespace SelfManagement.Application.DTO.Categories
{
    public class GetCategoriesRequest
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;
    }
}

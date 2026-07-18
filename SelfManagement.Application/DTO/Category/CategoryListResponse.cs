using System;

namespace SelfManagement.Application.DTO.Categories
{
    public class CategoryListResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

using SelfManagement.Application.DTO.Categories;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Domain.Entities;

namespace SelfManagement.Application.RepositoryInterface.Category
{
    public interface ICategoryRepository
    {
        Task<string> GenerateUniqueSlugAsync(string name);
        Task<bool> CreateCategoryAsync(SelfManagement.Domain.Entities.Category category);
        Task<PaginatedResponse<CategoryListResponse>> GetAllCategoriesAsync(GetCategoriesRequest request);
        Task<bool> UpdateCategoryAsync(SelfManagement.Domain.Entities.Category category);
        Task<SelfManagement.Domain.Entities.Category?> GetCategoryByIdAsync(Guid id);
        Task<bool> DeleteCategoryByIdAsync(Guid id);
    }
}

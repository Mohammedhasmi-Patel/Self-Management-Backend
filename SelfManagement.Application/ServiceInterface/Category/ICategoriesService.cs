using SelfManagement.Application.DTO.Categories;
using SelfManagement.Application.DTO.Common;

namespace SelfManagement.Application.ServiceInterface.Category
{
    public interface ICategoriesService
    {
        Task<ApiResponse<PaginatedResponse<CategoryListResponse>>> GetAllCategoriesAsync(GetCategoriesRequest request);
        Task<ApiResponse<object>> CreateCategoryAsync(CreateCategoryRequest request);
        Task<ApiResponse<object>> UpdateCategoryAsync(UpdateCategoryRequest request);
        Task<ApiResponse<object>> DeleteCategoryByIdAsync(Guid id);
    }
}

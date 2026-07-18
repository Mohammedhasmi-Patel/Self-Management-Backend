using SelfManagement.Application.DTO.Categories;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.RepositoryInterface.Category;
using SelfManagement.Application.RepositoryInterface.Common;
using SelfManagement.Application.ServiceInterface.Category;
using static SelfManagement.Application.Exceptions.BadRequestException;

namespace SelfManagement.Application.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<object>> CreateCategoryAsync(CreateCategoryRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();

            var category = new SelfManagement.Domain.Entities.Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Slug = await _categoryRepository.GenerateUniqueSlugAsync(request.Name)
            };

            var created = await _categoryRepository.CreateCategoryAsync(category);
            if (!created)
            {
                throw new InternalServerErrorExcdeption("Something went wrong while creating category");
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return ApiResponse<object>.SuccessResponse(null, "Category created successfully.");
        }

        public async Task<ApiResponse<object>> DeleteCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException("Category not found with given id.");
            }

            var response = await _categoryRepository.DeleteCategoryByIdAsync(id);
            return ApiResponse<object>.SuccessResponse(null, "Category deleted successfully.");
        }

        public async Task<ApiResponse<PaginatedResponse<CategoryListResponse>>> GetAllCategoriesAsync(GetCategoriesRequest request)
        {
            var response = await _categoryRepository.GetAllCategoriesAsync(request);
            return ApiResponse<PaginatedResponse<CategoryListResponse>>.SuccessResponse(response, "Categories list fetched successfully.");
        }

        public async Task<ApiResponse<object>> UpdateCategoryAsync(UpdateCategoryRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();

            var category = await _categoryRepository.GetCategoryByIdAsync(request.Id);
            if (category == null)
            {
                throw new NotFoundException("Category not found.");
            }

            category.Name = request.Name;
            category.Slug = await _categoryRepository.GenerateUniqueSlugAsync(request.Name);

            var updated = await _categoryRepository.UpdateCategoryAsync(category);
            if (!updated)
            {
                throw new InternalServerErrorExcdeption("Something went wrong while updating category");
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return ApiResponse<object>.SuccessResponse(null, "Category updated successfully.");
        }
    }
}

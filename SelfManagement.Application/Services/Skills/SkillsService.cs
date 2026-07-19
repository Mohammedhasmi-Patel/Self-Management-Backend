using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Skills;
using SelfManagement.Application.Exceptions;
using SelfManagement.Application.RepositoryInterface.Category;
using SelfManagement.Application.RepositoryInterface.Common;
using SelfManagement.Application.RepositoryInterface.Skill;
using SelfManagement.Application.ServiceInterface.Skill;
using SelfManagement.Application.Exceptions;

namespace SelfManagement.Application.Services.Skills
{
    public class SkillsService : ISkillsService
    {
        private readonly ISkillRepository _skillRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SkillsService(ISkillRepository skillRepository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _skillRepository = skillRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<object>> CreateSkillAsync(CreateSkillRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();

            var category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            var skillData = await _skillRepository.GetSkillByNameAsync(request.Name);

            if (skillData?.Name.ToLower() == request.Name.ToLower())
            {
                throw new BadRequestException("The skill name already exists.");
            }

            
            var skill = new SelfManagement.Domain.Entities.Skill
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Slug = await _skillRepository.GenerateUniqueSlugAsync(request.Name)
            };

            var created = await _skillRepository.CreateSkillAsync(skill);
            if (!created)
            {
                throw new InternalServerErrorExcdeption("Something went wrong while creating skill");
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return ApiResponse<object>.SuccessResponse(null, "Skill created successfully.");
        }

        public async Task<ApiResponse<object>> DeleteSkillByIdAsync(Guid id)
        {
            var skill = await _skillRepository.GetSkillByIdAsync(id);
            if (skill == null)
            {
                throw new NotFoundException("Skill not found with given id.");
            }

            var response = await _skillRepository.DeleteSkillByIdAsync(id);
            return ApiResponse<object>.SuccessResponse(null, "Skill deleted successfully.");
        }

        public async Task<ApiResponse<PaginatedResponse<SkillListResponse>>> GetAllSkillsAsync(GetSkillsRequest request)
        {
            var response = await _skillRepository.GetAllSkillsAsync(request);
            return ApiResponse<PaginatedResponse<SkillListResponse>>.SuccessResponse(response, "Skills list fetched successfully.");
        }

        public async Task<ApiResponse<object>> UpdateSkillAsync(UpdateSkillRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();

            var skill = await _skillRepository.GetSkillByIdAsync(request.Id);
            if (skill == null)
            {
                throw new NotFoundException("Skill not found.");
            }

            var category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            skill.Name = request.Name;
            skill.Description = request.Description;
            skill.CategoryId = request.CategoryId;
            skill.Slug = await _skillRepository.GenerateUniqueSlugAsync(request.Name);

            var updated = await _skillRepository.UpdateSkillAsync(skill);
            if (!updated)
            {
                throw new InternalServerErrorExcdeption("Something went wrong while updating skill");
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return ApiResponse<object>.SuccessResponse(null, "Skill updated successfully.");
        }
    }
}

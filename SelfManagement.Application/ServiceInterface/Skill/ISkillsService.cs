using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Skills;

namespace SelfManagement.Application.ServiceInterface.Skill
{
    public interface ISkillsService
    {
        Task<ApiResponse<PaginatedResponse<SkillListResponse>>> GetAllSkillsAsync(GetSkillsRequest request);
        Task<ApiResponse<object>> CreateSkillAsync(CreateSkillRequest request);
        Task<ApiResponse<object>> UpdateSkillAsync(UpdateSkillRequest request);
        Task<ApiResponse<object>> DeleteSkillByIdAsync(Guid id);
    }
}

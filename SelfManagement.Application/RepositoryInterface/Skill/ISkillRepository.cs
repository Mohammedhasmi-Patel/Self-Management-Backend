using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Skills;

namespace SelfManagement.Application.RepositoryInterface.Skill
{
    public interface ISkillRepository
    {
        Task<string> GenerateUniqueSlugAsync(string name);
        Task<bool> CreateSkillAsync(SelfManagement.Domain.Entities.Skill skill);
        Task<PaginatedResponse<SkillListResponse>> GetAllSkillsAsync(GetSkillsRequest request);
        Task<bool> UpdateSkillAsync(SelfManagement.Domain.Entities.Skill skill);
        Task<SelfManagement.Domain.Entities.Skill?> GetSkillByIdAsync(Guid id);
        Task<bool> DeleteSkillByIdAsync(Guid id);
        Task<SelfManagement.Domain.Entities.Skill?> GetSkillByNameAsync(string name);
    }
}

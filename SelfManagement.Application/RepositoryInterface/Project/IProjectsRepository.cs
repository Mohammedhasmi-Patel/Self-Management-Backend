using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Project;

namespace SelfManagement.Application.RepositoryInterface.Project
{
    public interface IProjectsRepository
    {
        Task<PaginatedResponse<ProjectListResponse>> GetAllProjectsByUserAsync(GetProjectsRequest request, Guid userId);
        Task<bool> CreateProjectAsync(SelfManagement.Domain.Entities.Project project);
        Task<bool> UpdateProjectAsync(SelfManagement.Domain.Entities.Project project);
        Task<SelfManagement.Domain.Entities.Project?> GetProjectByIdAsync(Guid id);
        Task<bool> DeleteProjectByIdAsync(Guid id);
        Task<string> GenerateUniqueSlugAsync(string name);
    }
}

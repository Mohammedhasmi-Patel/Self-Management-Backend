using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Project;

namespace SelfManagement.Application.ServiceInterface.Project
{
    public interface IProjectsService
    {
        Task<ApiResponse<PaginatedResponse<ProjectListResponse>>> GetAllProjectsByUserAsync(GetProjectsRequest request, Guid userId);
        Task<ApiResponse<object>> CreateProjectAsync(CreateProjectRequest request, Guid userId);
        Task<ApiResponse<object>> UpdateProjectAsync(UpdateProjectRequest request, Guid userId);
        Task<ApiResponse<object>> DeleteProjectByIdAsync(Guid id, Guid userId);
    }
}

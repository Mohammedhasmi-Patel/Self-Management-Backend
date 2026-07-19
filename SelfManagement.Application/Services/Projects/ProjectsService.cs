using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Project;
using SelfManagement.Application.RepositoryInterface.Common;
using SelfManagement.Application.RepositoryInterface.Company;
using SelfManagement.Application.RepositoryInterface.Project;
using SelfManagement.Application.ServiceInterface.Project;
using SelfManagement.Domain.Entities;
using SelfManagement.Application.Exceptions;

namespace SelfManagement.Application.Services.Projects
{
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectsRepository _projectsRepository;
        private readonly ICompaniesRepository _companiesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectsService(IProjectsRepository projectsRepository, ICompaniesRepository companiesRepository, IUnitOfWork unitOfWork)
        {
            _projectsRepository = projectsRepository;
            _companiesRepository = companiesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<object>> CreateProjectAsync(CreateProjectRequest request, Guid userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {

            var company = await _companiesRepository.GetCompanyByIdAsync(request.CompanyId);
            if (company == null)
            {
                throw new NotFoundException("Company not found");
            }

            Project project = new Project
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Slug = await _projectsRepository.GenerateUniqueSlugAsync(request.Name),
                CompanyId = request.CompanyId,
                RepositoryUrl = request.RepositoryUrl,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = request.Status ?? string.Empty,
                Notes = request.Notes,
                UserId = userId
            };

            var created = await _projectsRepository.CreateProjectAsync(project);
            if (!created)
            {
                throw new InternalServerErrorExcdeption("Something went wrong creating project");
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return ApiResponse<object>.SuccessResponse(null, "Project created successfully.");
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<ApiResponse<object>> DeleteProjectByIdAsync(Guid id, Guid userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
            var project = await _projectsRepository.GetProjectByIdAsync(id);
            if (project == null || project.Id == Guid.Empty)
            {
                throw new NotFoundException("Project not found with given id.");
            }

            if (project.UserId != userId)
            {
                throw new BadRequestException("You are not associated with this id");
            }

            var deleted = await _projectsRepository.DeleteProjectByIdAsync(id);
            if (!deleted)
            {
                throw new InternalServerErrorExcdeption("Something went wrong while deleting project");
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
            return ApiResponse<object>.SuccessResponse(null, "Project deleted successfully.");
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<ApiResponse<PaginatedResponse<ProjectListResponse>>> GetAllProjectsByUserAsync(GetProjectsRequest request, Guid userId)
        {
            var resp = await _projectsRepository.GetAllProjectsByUserAsync(request, userId);
            return ApiResponse<PaginatedResponse<ProjectListResponse>>.SuccessResponse(resp, "Projects fetched successfully.");
        }

        public async Task<ApiResponse<object>> UpdateProjectAsync(UpdateProjectRequest request, Guid userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {

            var project = await _projectsRepository.GetProjectByIdAsync(request.Id);
            if (project == null)
            {
                throw new NotFoundException("Project not found.");
            }

            if (project.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not allowed to update this project.");
            }

            var company = await _companiesRepository.GetCompanyByIdAsync(request.CompanyId);
            if (company == null)
            {
                throw new NotFoundException("Company not found.");
            }

            if (!string.Equals(project.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            {
                project.Slug = await _projectsRepository.GenerateUniqueSlugAsync(request.Name);
            }

            project.Name = request.Name;
            project.Description = request.Description;
            project.CompanyId = request.CompanyId;
            project.RepositoryUrl = request.RepositoryUrl;
            project.StartDate = request.StartDate;
            project.EndDate = request.EndDate;
            project.Status = request.Status ?? project.Status;
            project.Notes = request.Notes;
            project.UpdatedAt = DateTime.UtcNow;

            var updated = await _projectsRepository.UpdateProjectAsync(project);
            if (!updated)
            {
                throw new InternalServerErrorException("Something went wrong while updating project");
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return ApiResponse<object>.SuccessResponse(null, "Project updated successfully.");
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}

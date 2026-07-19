using Microsoft.EntityFrameworkCore;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Project;
using SelfManagement.Application.RepositoryInterface.Project;
using SelfManagement.Domain.Entities;
using SelfManagement.Infrastructure.Database;
using System.Text.RegularExpressions;

namespace SelfManagement.Infrastructure.Repository
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateUniqueSlugAsync(string name)
        {
            string baseSlug = Regex.Replace(name.Trim().ToLower(), @"[^a-z0-9\s-]", "")
                                   .Replace(" ", "-");
            baseSlug = Regex.Replace(baseSlug, "-+", "-");

            string slug = baseSlug;
            int count = 1;

            while (await _context.Projects.AnyAsync(c => c.Slug == slug))
            {
                slug = $"{baseSlug}-{count}";
                count++;
            }

            return slug;
        }

        public async Task<bool> CreateProjectAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<PaginatedResponse<ProjectListResponse>> GetAllProjectsByUserAsync(GetProjectsRequest request, Guid userId)
        {
            IQueryable<Project> query = _context.Projects
                                        .AsNoTracking()
                                        .Where(x => x.UserId == userId)
                                        .OrderBy(x => x.CreatedAt);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var s = request.Search.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(s) || (x.Description ?? string.Empty).ToLower().Contains(s));
            }

            int totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return new PaginatedResponse<ProjectListResponse>
                {
                    Items = Enumerable.Empty<ProjectListResponse>(),
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = 0,
                    TotalPages = 0
                };
            }

            List<ProjectListResponse> items = await query
                                                .Where(x => !x.IsDeleted)
                                                .Skip((request.PageNumber - 1) * request.PageSize)
                                                .Take(request.PageSize)
                                                .Select(x => new ProjectListResponse
                                                {
                                                    Id = x.Id,
                                                    Name = x.Name,
                                                    Slug = x.Slug,
                                                    Description = x.Description,
                                                    CompanyId = x.CompanyId,
                                                    RepositoryUrl = x.RepositoryUrl,
                                                    StartDate = x.StartDate,
                                                    EndDate = x.EndDate,
                                                    Status = x.Status,
                                                    Notes = x.Notes,
                                                    CreatedAt = x.CreatedAt
                                                })
                                                .ToListAsync();

            return new PaginatedResponse<ProjectListResponse>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
            };
        }

        public async Task<bool> UpdateProjectAsync(Project project)
        {
            _context.Projects.Update(project);
            int rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<Project?> GetProjectByIdAsync(Guid id)
        {
            return await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> DeleteProjectByIdAsync(Guid id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
            if (project == null) return false;

            project.IsDeleted = true;
            project.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Skills;
using SelfManagement.Application.RepositoryInterface.Skill;
using SelfManagement.Domain.Entities;
using SelfManagement.Infrastructure.Database;
using System.Text.RegularExpressions;

namespace SelfManagement.Infrastructure.Repository
{
    public class SkillRepository : ISkillRepository
    {
        private readonly ApplicationDbContext _context;

        public SkillRepository(ApplicationDbContext context)
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

            while (await _context.Skills.AnyAsync(c => c.Slug == slug))
            {
                slug = $"{baseSlug}-{count}";
                count++;
            }

            return slug;
        }

        public async Task<bool> CreateSkillAsync(SelfManagement.Domain.Entities.Skill skill)
        {
            await _context.Skills.AddAsync(skill);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<PaginatedResponse<SkillListResponse>> GetAllSkillsAsync(GetSkillsRequest request)
        {
            IQueryable<Skill> query = _context.Skills
                                        .AsNoTracking()
                                        .Where(x => !x.IsDeleted)
                                        .OrderBy(x => x.CreatedAt);

            int totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return new PaginatedResponse<SkillListResponse>
                {
                    Items = Enumerable.Empty<SkillListResponse>(),
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = 0,
                    TotalPages = 0
                };
            }

            var items = await query
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .Select(x => new SkillListResponse
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Slug = x.Slug,
                            Description = x.Description,
                            CategoryId = x.CategoryId,
                            CreatedAt = x.CreatedAt
                        })
                        .ToListAsync();

            return new PaginatedResponse<SkillListResponse>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
            };
        }

        public async Task<bool> UpdateSkillAsync(SelfManagement.Domain.Entities.Skill skill)
        {
            _context.Skills.Update(skill);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<SelfManagement.Domain.Entities.Skill?> GetSkillByIdAsync(Guid id)
        {
            return await _context.Skills.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> DeleteSkillByIdAsync(Guid id)
        {
            var skill = await _context.Skills.FirstOrDefaultAsync(x => x.Id == id);
            if (skill == null) return false;

            skill.IsDeleted = true;
            skill.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Skill?> GetSkillByNameAsync(string name)
        {
            return await _context.Skills.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()) ?? null;
            
        }
    }
}

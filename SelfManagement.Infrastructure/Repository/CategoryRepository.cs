using Microsoft.EntityFrameworkCore;
using SelfManagement.Application.DTO.Categories;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.RepositoryInterface.Category;
using SelfManagement.Domain.Entities;
using SelfManagement.Infrastructure.Database;
using System.Text.RegularExpressions;

namespace SelfManagement.Infrastructure.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
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

            while (await _context.Categories.AnyAsync(c => c.Slug == slug))
            {
                slug = $"{baseSlug}-{count}";
                count++;
            }

            return slug;
        }

        public async Task<bool> CreateCategoryAsync(SelfManagement.Domain.Entities.Category category)
        {
            await _context.Categories.AddAsync(category);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<PaginatedResponse<CategoryListResponse>> GetAllCategoriesAsync(GetCategoriesRequest request)
        {
            IQueryable<Category> query = _context.Categories
                                        .AsNoTracking()
                                        .Where(x => !x.IsDeleted)
                                        .OrderBy(x => x.CreatedAt);

            int totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return new PaginatedResponse<CategoryListResponse>
                {
                    Items = Enumerable.Empty<CategoryListResponse>(),
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = 0,
                    TotalPages = 0
                };
            }

            var items = await query
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .Select(x => new CategoryListResponse
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Slug = x.Slug,
                            CreatedAt = x.CreatedAt
                        })
                        .ToListAsync();

            return new PaginatedResponse<CategoryListResponse>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
            };
        }

        public async Task<bool> UpdateCategoryAsync(SelfManagement.Domain.Entities.Category category)
        {
            _context.Categories.Update(category);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<SelfManagement.Domain.Entities.Category?> GetCategoryByIdAsync(Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> DeleteCategoryByIdAsync(Guid id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null) return false;

            category.IsDeleted = true;
            category.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}

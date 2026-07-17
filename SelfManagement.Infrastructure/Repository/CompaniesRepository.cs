
using Microsoft.EntityFrameworkCore;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Company;
using SelfManagement.Application.RepositoryInterface.Company;
using SelfManagement.Domain.Entities;
using SelfManagement.Infrastructure.Database;
using System.Text.RegularExpressions;

namespace SelfManagement.Infrastructure.Repository
{
    public class CompaniesRepository : ICompaniesRepository
    {
        private readonly ApplicationDbContext _context;

        public CompaniesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public  async Task<string> GenerateUniqueSlugAsync(string name)
        {
            // Create base slug
            string baseSlug = Regex.Replace(name.Trim().ToLower(), @"[^a-z0-9\s-]", "")
                                   .Replace(" ", "-");
            baseSlug = Regex.Replace(baseSlug, "-+", "-");

            string slug = baseSlug;
            int count = 1;

            while (await _context.Companies.AnyAsync(c => c.Slug == slug))
            {
                slug = $"{baseSlug}-{count}";
                count++;
            }

            return slug;
        }

        public async Task<bool> CreateCompanyAsync(Company company)
        {
            await _context.Companies.AddAsync(company);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<PaginatedResponse<CompanyListResponse>> GetAllCompaniesByUserAsync(GetCompaniesRequest request,Guid userId)
        {
            IQueryable<Company> query = _context.Companies
                                        .AsNoTracking()
                                        .Where(x => x.UserId == userId)
                                        .OrderBy(x => x.CreatedAt);

            int totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return new PaginatedResponse<CompanyListResponse>
                {
                    Items = Enumerable.Empty<CompanyListResponse>(),
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = 0,
                    TotalPages = 0
                };
            }

            List<CompanyListResponse> companies = await query
                                                .Where(x => !x.IsDeleted)
                                                .Skip((request.PageNumber - 1) * request.PageSize)
                                                .Take(request.PageSize)
                                                .Select(x => new CompanyListResponse()
                                                {
                                                    Id = x.Id,
                                                    Name = x.Name,
                                                    Description = x.Description,
                                                    LogoUrl = x.LogoUrl,
                                                    WebsiteUrl = x.WebsiteUrl,
                                                    PhoneNumber = x.PhoneNumber!,
                                                    Email = x.Email,
                                                    Address = x.Address,
                                                    CountryId = x.CountryId,
                                                    StateId = x.StateId,
                                                    CityId = x.CityId,
                                                    CreatedAt = x.CreatedAt
                                                })
                                                .ToListAsync();
            return new PaginatedResponse<CompanyListResponse>()
            {
                Items = companies,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
            };
        }

        public async Task<bool> UpdateCompanyAsync(Company company)
        {
            _context.Companies.Update(company);

            int rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<Company?> GetCompanyByIdAsync(Guid id)
        {
           return await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> DeleteCompanyByIdAsync(Guid id)
        {
            Company company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);
            if (company == null) return false;

            company.IsActive = false;
            company.IsDeleted = true;
            company.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;

        }
    }
}

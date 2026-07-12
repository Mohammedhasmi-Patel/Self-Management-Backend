
using Microsoft.EntityFrameworkCore;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Company;
using SelfManagement.Application.RepositoryInterface.Company;
using SelfManagement.Domain.Entities;
using SelfManagement.Infrastructure.Database;

namespace SelfManagement.Infrastructure.Repository
{
    public class CompaniesRepository : ICompaniesRepository
    {
        private readonly ApplicationDbContext _context;

        public CompaniesRepository(ApplicationDbContext context)
        {
            _context = context;
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
                    Items = System.Linq.Enumerable.Empty<CompanyListResponse>(),
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = 0,
                    TotalPages = 0
                };
            }

            List<CompanyListResponse> companies = await query
                                                .Skip((request.PageNumber - 1) * request.PageSize)
                                                .Take(request.PageSize)
                                                .Select(x => new CompanyListResponse()
                                                {
                                                    Id = x.Id,
                                                    Name = x.Name,
                                                    Address = x.Address!,
                                                    LogoUrl = x.LogoUrl
                                                    ,CreatedAt = x.CreatedAt
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
    }
}

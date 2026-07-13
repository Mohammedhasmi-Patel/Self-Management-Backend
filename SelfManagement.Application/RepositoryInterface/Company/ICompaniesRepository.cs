
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Company;

namespace SelfManagement.Application.RepositoryInterface.Company
{
    public interface ICompaniesRepository
    {
        public Task<PaginatedResponse<CompanyListResponse>> GetAllCompaniesByUserAsync(GetCompaniesRequest request, Guid userId);
        public Task<bool> CreateCompanyAsync(SelfManagement.Domain.Entities.Company company);
        Task<string> GenerateUniqueSlugAsync(string name);

    }
}

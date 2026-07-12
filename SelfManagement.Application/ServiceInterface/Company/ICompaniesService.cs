
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Company;

namespace SelfManagement.Application.ServiceInterface.Company
{
    public interface ICompaniesService
    {
        public Task<ApiResponse<PaginatedResponse<CompanyListResponse>>> GetAllCompaniesByUserAsync(GetCompaniesRequest request,Guid userId);
    }
}

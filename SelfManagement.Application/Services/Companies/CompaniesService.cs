
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Company;
using SelfManagement.Application.RepositoryInterface.Company;
using SelfManagement.Application.ServiceInterface.Company;

namespace SelfManagement.Application.Services.Companies
{
    public class CompaniesService : ICompaniesService
    {
        private readonly ICompaniesRepository _companiesRepository;
          
        public CompaniesService(ICompaniesRepository companiesRepository)
        {
            _companiesRepository = companiesRepository;
        }
        public async Task<ApiResponse<PaginatedResponse<CompanyListResponse>>> GetAllCompaniesByUserAsync(GetCompaniesRequest request,Guid userId)
        {
            PaginatedResponse<CompanyListResponse> response = await _companiesRepository.GetAllCompaniesByUserAsync(request,userId);
            return  ApiResponse<PaginatedResponse<CompanyListResponse>>.SuccessResponse(response);
        }
    }
}

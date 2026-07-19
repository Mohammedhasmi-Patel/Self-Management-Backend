using Microsoft.AspNetCore.Mvc;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Company;
using SelfManagement.Application.RepositoryInterface.Common;
using SelfManagement.Application.RepositoryInterface.Company;
using SelfManagement.Application.RepositoryInterface.Locations;
using SelfManagement.Application.ServiceInterface.Company;
using SelfManagement.Application.ServiceInterface.FileStorage;
using SelfManagement.Domain.Entities;
// Exceptions namespace
using SelfManagement.Application.Exceptions;

namespace SelfManagement.Application.Services.Companies
{
    public class CompaniesService : ICompaniesService
    {
        private readonly ICompaniesRepository _companiesRepository;
        private readonly IFileService _fileService;
        private readonly ICountriesRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICitiesRepository _cityRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompaniesService(ICompaniesRepository companiesRepository,IFileService fileService, ICountriesRepository countriesRepository, IStateRepository stateRepository, ICitiesRepository cityRepository, IUnitOfWork unitOfWork)
        {
            _companiesRepository = companiesRepository;
            _fileService = fileService;
            _countryRepository = countriesRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<object>> CreateCompanyAsync(CreateCompanyRequest request,Guid userId)
        {
            await _unitOfWork.BeginTransactionAsync();

            Guid countryId = request.CountryId;
            var isCountryExist = await _countryRepository.IsCompanyExistAsync(countryId);
            if (!isCountryExist)
            {
                throw new NotFoundException("Country not found");
            }
            Guid stateId = request.StateId;

            var stateData = await _stateRepository.GetStateByIdAsync(stateId);
            if (stateData == null || stateData?.Id == null)
            {
                throw new NotFoundException("State not found");
            }

            if(stateData.CountryId != countryId)
            {
                throw new BadRequestException("The state is not belong to this country");
            }

            Guid cityId = request.CityId;
            var cityData = await _cityRepository.GetCityByIdAsync(cityId);

            if (cityData is null)
            {
                throw new NotFoundException("City not found");
            }

            if (cityData.StateId != stateId)
            {
                throw new BadRequestException("The given city doesnt belongs to the state");
            }

            var result = await _fileService.SaveFileAsync( request.Logo,"companies",new[] { ".jpg", ".jpeg", ".png", ".webp" },5*1024*1024);
            //var result = await _fileService.SaveFileAsync(request.Logo, "uploads/company");
            Company company = new Company()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = request.Name,
                Description = request.Description,
                Slug = await _companiesRepository.GenerateUniqueSlugAsync(request.Name),
                LogoUrl = result.FilePath,
                WebsiteUrl = request.WebsiteUrl,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                CountryId = request.CountryId,
                StateId = request.StateId,
                CityId = request.CityId,
            };

            var response = await _companiesRepository.CreateCompanyAsync(company);
            if (response == false)
            {
                throw new InternalServerErrorExcdeption("Something went wrong while creating company");
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return ApiResponse<object>.SuccessResponse(null,"Company created successfully.");
        }

        public async Task<ApiResponse<object>> DeleteCompanyByIdAsync(Guid id, Guid userId)
        {
            var res = await _companiesRepository.GetCompanyByIdAsync(id);
            if (res == null || res.Id == Guid.Empty)
            {
                throw new NotFoundException("Company not found with given id.");
            }

            if(res.UserId != userId)
            {
                throw new BadRequestException("You are not associated with this id");
            }

            var response = await _companiesRepository.DeleteCompanyByIdAsync(id);
            return ApiResponse<object>.SuccessResponse(null,"Company deleted successfully.");
        }

        public async Task<ApiResponse<PaginatedResponse<CompanyListResponse>>> GetAllCompaniesByUserAsync(GetCompaniesRequest request,Guid userId)
        {
            PaginatedResponse<CompanyListResponse> response = await _companiesRepository.GetAllCompaniesByUserAsync(request,userId);

            foreach (var item in response.Items)
            {
                item.LogoUrl = _fileService.GetFilePath(item.LogoUrl);
            }

            return  ApiResponse<PaginatedResponse<CompanyListResponse>>.SuccessResponse(response,"Companies list fetched successfully.");
        }

        public async Task<ApiResponse<object>> UpdateCompanyAsync(CompanyUpdateRequest request, Guid userId)
        {
            await _unitOfWork.BeginTransactionAsync();

            var company = await _companiesRepository.GetCompanyByIdAsync(request.Id);

            if (company == null)
            {
                throw new NotFoundException("Company not found.");
            }

            if (company.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not allowed to update this company.");
            }

            // Country Validation
            var isCountryExist = await _countryRepository.IsCompanyExistAsync(request.CountryId);
            if (!isCountryExist)
            {
                throw new NotFoundException("Country not found.");
            }

            // State Validation
            var stateData = await _stateRepository.GetStateByIdAsync(request.StateId);
            if (stateData == null)
            {
                throw new NotFoundException("State not found.");
            }

            if (stateData.CountryId != request.CountryId)
            {
                throw new BadRequestException("The state does not belong to this country.");
            }

            // City Validation
            var cityData = await _cityRepository.GetCityByIdAsync(request.CityId);
            if (cityData == null)
            {
                throw new NotFoundException("City not found.");
            }

            if (cityData.StateId != request.StateId)
            {
                throw new BadRequestException("The city does not belong to this state.");
            }

            // Upload new logo if provided
            if (request.Logo != null)
            {
                if (!string.IsNullOrEmpty(company.LogoUrl))
                {
                    _fileService.DeleteFile(company.LogoUrl!);
                }

                var result = await _fileService.SaveFileAsync(
                    request.Logo,
                    "companies",
                    new[] { ".jpg", ".jpeg", ".png", ".webp" },
                    5 * 1024 * 1024);

                company.LogoUrl = result.FilePath;
            }

            // Update properties
            if (!string.Equals(company.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            {
                company.Slug = await _companiesRepository.GenerateUniqueSlugAsync(request.Name);
            }

            company.Name = request.Name;
            company.Description = request.Description;
            company.WebsiteUrl = request.WebsiteUrl;
            company.Email = request.Email;
            company.PhoneNumber = request.PhoneNumber;
            company.Address = request.Address;
            company.CountryId = request.CountryId;
            company.StateId = request.StateId;
            company.CityId = request.CityId;
            company.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return ApiResponse<object>.SuccessResponse(null, "Company updated successfully.");
        }
    }
}

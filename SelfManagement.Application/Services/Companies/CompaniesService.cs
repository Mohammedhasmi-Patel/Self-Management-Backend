
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Company;
using SelfManagement.Application.Exceptions;
using SelfManagement.Application.RepositoryInterface.Company;
using SelfManagement.Application.RepositoryInterface.Locations;
using SelfManagement.Application.ServiceInterface.Company;
using SelfManagement.Application.ServiceInterface.FileStorage;
using SelfManagement.Domain.Entities;
using static SelfManagement.Application.Exceptions.BadRequestException;

namespace SelfManagement.Application.Services.Companies
{
    public class CompaniesService : ICompaniesService
    {
        private readonly ICompaniesRepository _companiesRepository;
        private readonly IFileService _fileService;
        private readonly ICountriesRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;



        public CompaniesService(ICompaniesRepository companiesRepository,IFileService fileService, ICountriesRepository countriesRepository, IStateRepository stateRepository, ICityRepository cityRepository)
        {
            _companiesRepository = companiesRepository;
            _fileService = fileService;
            _countryRepository = countriesRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
        }

        public async Task<ApiResponse<object>> CreateCompanyAsync(CreateCompanyRequest request,Guid userId)
        {
            Guid countryId = request.CountryId;
            var isCountryExist = await _countryRepository.IsCompanyExistAsync(countryId);
            if (!isCountryExist)
            {
                throw new NotFoundException("Country not found");
            }
            Guid stateId = request.StateId;

            var isStateExist = await _stateRepository.IsStateExistAsync(stateId);
            if (!isStateExist)
            {
                throw new NotFoundException("State not found");
            }

            Guid cityId = request.CountryId;
            var isCityExist = await _cityRepository.IsCityExistAsync(cityId);

            if (!isCityExist)
            {
                throw new NotFoundException(" not found");
            }



            var result = await _fileService.UploadFileAsync( request.Logo,"companies",new[] { ".jpg", ".jpeg", ".png", ".webp" },5);
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

            return ApiResponse<object>.SuccessResponse(null,"Company created successfully.");
        }

        public async Task<ApiResponse<PaginatedResponse<CompanyListResponse>>> GetAllCompaniesByUserAsync(GetCompaniesRequest request,Guid userId)
        {
            PaginatedResponse<CompanyListResponse> response = await _companiesRepository.GetAllCompaniesByUserAsync(request,userId);
            return  ApiResponse<PaginatedResponse<CompanyListResponse>>.SuccessResponse(response);
        }
    }
}

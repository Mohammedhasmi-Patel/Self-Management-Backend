

using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Locations.Country;
using SelfManagement.Application.RepositoryInterface.Locations;
using SelfManagement.Application.ServiceInterface.Location;

namespace SelfManagement.Application.Services.Location
{
    public class CountriesService : ICountriesService
    {
        private readonly ICountriesRepository _countryRepo;

        public CountriesService(ICountriesRepository countriesRepository)
        {
            _countryRepo = countriesRepository;
        }
        public async Task<ApiResponse<List<CountryListDropdown>>> GetCountryListDropdownsAsync()
        {
           var res =  await _countryRepo.GetCountryListDropdownAsync();
            return ApiResponse<List<CountryListDropdown>>.SuccessResponse(res,"Countries fetch successfully.");
        }
    }
}


using SelfManagement.Application.DTO.Locations.Country;
using SelfManagement.Application.DTO.Common;


namespace SelfManagement.Application.ServiceInterface.Location
{
    public interface ICountriesService
    {
        public Task<ApiResponse<List<CountryListDropdown>>> GetCountryListDropdownsAsync();
    }
}

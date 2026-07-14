using SelfManagement.Application.DTO.Locations.Country;

namespace SelfManagement.Application.RepositoryInterface.Locations
{
    public interface ICountriesRepository
    {
        public  Task<bool> IsCompanyExistAsync(Guid id);
        public Task<List<CountryListDropdown>> GetCountryListDropdownAsync();
    }
}

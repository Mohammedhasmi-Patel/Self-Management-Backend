


using SelfManagement.Application.DTO.Locations.City;

namespace SelfManagement.Application.RepositoryInterface.Locations
{
    public interface ICitiesRepository
    {
        public Task<bool> IsCityExistAsync(Guid id);
        public Task<List<CityListDropdown>> GetCitiesByStateIdAsync(Guid id);
        public Task<CityResponse?> GetCityByIdAsync(Guid id);

    }
}

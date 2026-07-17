

using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Locations.City;

namespace SelfManagement.Application.ServiceInterface.Location
{
    public interface ICitiesService
    {
        public Task<ApiResponse<List<CityListDropdown>>> GetCitiesByStateIdAsync(Guid id);
        public Task<CityResponse?> GetCitiesByIdAsync(Guid id);
    }
}

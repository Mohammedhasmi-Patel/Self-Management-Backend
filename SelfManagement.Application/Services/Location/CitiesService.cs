
using Microsoft.AspNetCore.Http.HttpResults;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Locations.City;
using SelfManagement.Application.RepositoryInterface.Locations;
using SelfManagement.Application.ServiceInterface.Location;
using static SelfManagement.Application.Exceptions.BadRequestException;

namespace SelfManagement.Application.Services.Location
{
    public class CitiesService : ICitiesService
    {
        private readonly ICitiesRepository _citiesRepository;
        private readonly IStateRepository _stateRepository;

        public CitiesService(ICitiesRepository citiesRepository, IStateRepository stateRepository)
        {
            _citiesRepository = citiesRepository;
            _stateRepository  = stateRepository;
        }

        public Task<CityResponse?> GetCitiesByIdAsync(Guid id)
        {
            return _citiesRepository.GetCityByIdAsync(id);
        }

        public async Task<ApiResponse<List<CityListDropdown>>> GetCitiesByStateIdAsync(Guid id)
        {
            bool isStateExist = await _stateRepository.IsStateExistAsync(id);
            if (!isStateExist)
            {
                throw new NotFoundException("State not found.");
            }
            var response = await _citiesRepository.GetCitiesByStateIdAsync(id);
            return ApiResponse<List<CityListDropdown>>.SuccessResponse(response, "Cities fetched successfully.");
        }
    }
}



using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Locations.States;
using SelfManagement.Application.RepositoryInterface.Locations;
using SelfManagement.Application.ServiceInterface.Location;
using static SelfManagement.Application.Exceptions.BadRequestException;

namespace SelfManagement.Application.Services.Location
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;
        private readonly ICountriesRepository _countriesRepository;

        public StateService(IStateRepository stateRepository,ICountriesRepository countriesRepository)
        {
            _stateRepository = stateRepository;
            _countriesRepository = countriesRepository;
        }

        public async Task<StateResponse?> GetStateByIdAsdync(Guid id)
        {
            return await _stateRepository.GetStateByIdAsync(id);
        }

        public async Task<ApiResponse<List<StateListDropdown>>> GetStatesByCountryIdAsync(Guid id)
        {
            var isValidCountry = await _countriesRepository.IsCompanyExistAsync(id);
            if (!isValidCountry)
            {
                throw new NotFoundException("Country not found");
            }
           var response = await _stateRepository.GetStatesByCountryIdAsync(id);
            return ApiResponse<List<StateListDropdown>>.SuccessResponse(response,"State list found successfully.");
        }
    }
}

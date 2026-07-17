

using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Locations.States;

namespace SelfManagement.Application.ServiceInterface.Location
{
    public interface IStateService
    {
        public Task<ApiResponse<List<StateListDropdown>>> GetStatesByCountryIdAsync(Guid id);
        public Task<StateResponse?> GetStateByIdAsdync(Guid id);

    }
}

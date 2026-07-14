

using SelfManagement.Application.DTO.Locations.States;

namespace SelfManagement.Application.RepositoryInterface.Locations
{
    public interface IStateRepository
    {
        public Task<bool> IsStateExistAsync(Guid id);
        public Task<List<StateListDropdown>> GetStatesByCountryIdAsync(Guid id);

    }
}

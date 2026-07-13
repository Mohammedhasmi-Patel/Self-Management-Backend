

namespace SelfManagement.Application.RepositoryInterface.Locations
{
    public interface IStateRepository
    {
        public Task<bool> IsStateExistAsync(Guid id);

    }
}

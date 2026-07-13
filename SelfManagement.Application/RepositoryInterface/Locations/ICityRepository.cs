

namespace SelfManagement.Application.RepositoryInterface.Locations
{
    public interface ICityRepository
    {
        public Task<bool> IsCityExistAsync(Guid id);

    }
}

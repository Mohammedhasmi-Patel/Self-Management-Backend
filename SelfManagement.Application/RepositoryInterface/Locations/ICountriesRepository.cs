

namespace SelfManagement.Application.RepositoryInterface.Locations
{
    public interface ICountriesRepository
    {
        public  Task<bool> IsCompanyExistAsync(Guid id); 
    }
}


using Microsoft.EntityFrameworkCore;
using SelfManagement.Application.RepositoryInterface.Locations;
using SelfManagement.Infrastructure.Database;

namespace SelfManagement.Infrastructure.Repository.Location
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly ApplicationDbContext _context;

        public CountriesRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> IsCompanyExistAsync(Guid id)
        {
            Guid idI = id;
           return  await _context.Countries.AnyAsync(c => c.Id == id);
        }
    }
}
